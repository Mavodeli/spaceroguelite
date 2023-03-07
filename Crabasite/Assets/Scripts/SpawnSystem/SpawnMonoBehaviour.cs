using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnMonoBehaviour : MonoBehaviour
{
    public delegate void addEnemyBehaviour(GameObject enemy);
    private bool loading_scene;
    private GameObject object_to_move;
    private string scene_to_move_to;

    public static void Enemy(string type, Vector3 position){
        Dictionary<string, addEnemyBehaviour> EnemyBehaviourMap = new Dictionary<string, addEnemyBehaviour>();
        EnemyBehaviourMap.Add("PufferFish", delegate(GameObject e){e.AddComponent<PufferFishBehaviour>();});
        EnemyBehaviourMap.Add("AnglerFish", delegate(GameObject e){e.AddComponent<AnglerFishBehaviour>();});
        EnemyBehaviourMap.Add("MorayEel", delegate(GameObject e){e.AddComponent<MorayEelBehaviour>();});
        EnemyBehaviourMap.Add("MantisShrimp", delegate(GameObject e){e.AddComponent<MantisShrimpBehaviour>();});

        GameObject enemy = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
        EnemyBehaviourMap[type](enemy);
        enemy.transform.position = position;
    }

    public void Item(string type, Vector3 position, string scene){
        GameObject item = Object.Instantiate(Resources.Load<GameObject>("Prefabs/DefaultItem"));
        Item so = Resources.Load<Item>("ScriptableObjects/Items/"+type);

        SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
        sr.sprite = so.icon;
        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size *= so.iconScale;

        ItemBehaviour script = item.AddComponent<ItemBehaviour>();
        item.transform.position = position;
        item.name = so.itemName;

        // object_to_move = item;
        // scene_to_move_to = scene;
        // loading_scene = true;
    }

    public static void Mail(string id){
        InventoryManager IM = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
        IM.AddMail(id);
    }

    public static void Quest(string id){
        InventoryManager IM = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
        GameObject GH = GameObject.FindGameObjectWithTag("GameHandler");
        IM.AddQuestDescription(id);
        GH.SendMessage("addNewQuest", id, SendMessageOptions.DontRequireReceiver);
    }

    public static void NewSprite(string name, GameObject go){
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(name);
        sr.sprite = sprite;
    }

    void Update(){
        if(loading_scene){
            StartCoroutine(MoveObjectToSceneAsync(object_to_move, scene_to_move_to));
        }
    }

    IEnumerator MoveObjectToSceneAsync(GameObject obj, string scene)
    {
        // Set the current Scene to be able to unload it later
        // Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(scene));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(scene));

        object_to_move = null;
        scene_to_move_to = null;
        loading_scene = false;
    }
}
