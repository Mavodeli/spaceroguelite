using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using log4net.Core;

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

    private bool QuestIsCompleted(string id){
        bool b = false;
        try
        {
            b = !gameData.activeQuests[id];
        }
        catch(KeyNotFoundException){}
        catch(System.NullReferenceException){}
        return b;
    }

    public bool ProgressionFlagIsSet(string id){
        bool b = false;
        try
        {
            b = gameData.ProgressionDict[id];
        }
        catch(KeyNotFoundException){}
        catch(System.NullReferenceException){}
        return b;
    }

    public void setProgressionFlag(string id, bool value = true){
        if(HasGameData())
            gameData.ProgressionDict[id] = value;
    }

    // Called when switching Scenes
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame(false);

        //makeshift sprite changer
        //maybeTODO: obsolete when level state persistence is implemented ;)
        Sprite sprite = Resources.Load<Sprite>(ConstructSpriteString.Spaceship(
            scene.name,
            QuestIsCompleted("RepairWindshield"),
            QuestIsCompleted("RepairSpaceship"),
            QuestIsCompleted("InstallNewHyperdriveCore")
        ));

        GameObject hull = null;
        try
        {
            hull = GameObject.FindGameObjectWithTag("ShipHull");
        }
        catch (System.NullReferenceException){}

        if(hull != null)
            hull.GetComponent<SpriteRenderer>().sprite = sprite;


        GameObject GameHandler = null;

        try
        {
           GameHandler = GameObject.FindGameObjectWithTag("GameHandler");
        }
        catch (System.NullReferenceException) { }

        if(GameHandler != null && SceneManager.GetActiveScene().name != "Level 0 - spaceship")
        {
            GameHandler.GetComponent<CollectibleTracking>().deleteCollectibles();
        }
            

    }
    // Called when switching Scenes
    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame(false);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        SaveGame(true);//resets save file
        Debug.Log("New Game");
    }
    public void LoadGame(bool fromFile)
    {
        if(fromFile){
            this.gameData = dataHandler.Load();

            //reset all enemySpawn flags if loaded from file
            Dictionary<string, bool> copy = gameData.ProgressionDict;
            List<int> spawn_indices = new List<int>();
            int i = 0;
            foreach(KeyValuePair<string, bool> entry in gameData.ProgressionDict){
                string[] substrings = entry.Key.Split("_");
                if(substrings[0] == "EnemySpawn")
                    spawn_indices.Add(i);
                i++;
            }
            foreach(int idx in spawn_indices){
                gameData.ProgressionDict[gameData.ProgressionDict.ElementAt<KeyValuePair<string, bool>>(idx).Key] = false; 
            }
        }
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

    public GameData getGameData(bool fromFile = false){
        if(HasGameData())
            return gameData;
        if(fromFile)
            return dataHandler.Load();
        return null;
    }
}
