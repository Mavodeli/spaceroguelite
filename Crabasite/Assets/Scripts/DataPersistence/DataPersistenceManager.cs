using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; set; }

    private void Awake()
    {
        if (instance != null)
        {
            // Debug.LogError("Found more than one Data Persistance Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        // TODO LoadGame on Death or LoadGame on Button press aswell.
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    // Called when switching Scenes
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame(false);

        //makeshift sprite changer
        //maybeTODO: obsolete when level state persistence is implemented ;)
        bool completed = false;
        try{ completed = !gameData.activeQuests["RepairSpaceship"]; }
        catch(KeyNotFoundException){}
        catch(System.NullReferenceException){}

        Sprite sprite = null;
        if(completed && scene.name == "Level 1 - space")
            sprite = Resources.Load<Sprite>("Sprites/Spaceship/clean_exterior");
        else if(completed && scene.name == "Level 0 - spaceship")
            sprite = Resources.Load<Sprite>("Sprites/Spaceship/clean_interior");
        else if(!completed && scene.name == "Level 1 - space")
            sprite = Resources.Load<Sprite>("Sprites/Spaceship/broken_exterior");
        else if(!completed && scene.name == "Level 0 - spaceship")
            sprite = Resources.Load<Sprite>("Sprites/Spaceship/broken_interior");

        if(sprite != null)
            GameObject.FindGameObjectWithTag("ShipHull").GetComponent<SpriteRenderer>().sprite = sprite;

    }
    // Called when switching Scenes
    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame(false);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        Debug.Log("New Game");
    }
    public void LoadGame(bool fromFile)
    {
        if(fromFile) this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A New Game has to be started first.");
            return;
            //NewGame();
        }
        
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }
    public void SaveGame(bool toFile)
    {
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        if(toFile) dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        //TODO change later to Save at specific time in Spaceship, so save on button press.
        // SaveGame();
        // Debug.Log("Saved Game");
    }
    

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<Object>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public GameData getGameData(){
        if(HasGameData())
            return gameData;
        return null;
    }
}
