using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn
{
    public delegate void addEnemyBehaviour(GameObject enemy);
    InventoryManager IM = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();

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

    public static void Item(string type, Vector3 position, PhysicalEntity.OnPickup onPickup = null){
        GameObject item = Object.Instantiate(Resources.Load<GameObject>("Prefabs/DefaultItem"));
        ItemBehaviour script = item.AddComponent<ItemBehaviour>();
        script.Setup(type, onPickup);
        item.transform.position = position;
    }

    public static void Mail(string id){
        IM.AddMail(id);
    }

    public static void QuestDescription(string id){
        IM.AddQuestDescription(id);
    }
}
