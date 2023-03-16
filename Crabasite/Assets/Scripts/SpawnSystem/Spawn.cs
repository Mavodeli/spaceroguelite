using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn
{
    public delegate void addEnemyBehaviour(GameObject enemy);

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

    public static void Item(string type, Vector3 position, string scene){
        GameObject item = Object.Instantiate(Resources.Load<GameObject>("Prefabs/DefaultItem"));
        Item so = Resources.Load<Item>("ScriptableObjects/Items/"+type);

        ItemBehaviour script = item.AddComponent<ItemBehaviour>();
        script.Setup(type);

        item.transform.position = position;
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
}