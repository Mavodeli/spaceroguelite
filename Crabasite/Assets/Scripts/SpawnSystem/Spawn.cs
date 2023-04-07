using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn
{
    public delegate void addEnemyBehaviour(GameObject enemy);

    public static void Enemy(string type, Vector3 position, string[] itemsToDrop = null){
        Dictionary<string, addEnemyBehaviour> EnemyBehaviourMap = new Dictionary<string, addEnemyBehaviour>();
        EnemyBehaviourMap.Add("PufferFish", delegate(GameObject e){e.AddComponent<PufferFishBehaviour>();});
        EnemyBehaviourMap.Add("AnglerFish", delegate(GameObject e){e.AddComponent<AnglerFishBehaviour>();});
        EnemyBehaviourMap.Add("MorayEel", delegate(GameObject e){e.AddComponent<MorayEelBehaviour>();});
        EnemyBehaviourMap.Add("MantisShrimp", delegate(GameObject e){e.AddComponent<MantisShrimpBehaviour>();});

        GameObject enemy = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
        EnemyBehaviourMap[type](enemy);
        enemy.transform.position = position;
        itemsToDrop = itemsToDrop == null ? new string[]{} : itemsToDrop;
        enemy.SendMessage("setItemsToDrop", itemsToDrop, SendMessageOptions.DontRequireReceiver);
    }

    public static void Seelie(Vector3 position, bool drop_ultimate){
        GameObject seelie = Object.Instantiate(Resources.Load<GameObject>("Prefabs/AbandonedSpaceship/Seelie"));
        seelie.transform.position = position;
        seelie.SendMessage("shouldDropUltimate", drop_ultimate, SendMessageOptions.DontRequireReceiver);
    }

    public static void Item(string type, Vector3 position, ItemBehaviour.OnPickup onPickup = null){
        GameObject item = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Collectables/DefaultItem"));
        Item so = Resources.Load<Item>("ScriptableObjects/Items/"+type);

        ItemBehaviour script = item.AddComponent<ItemBehaviour>();
        script.Setup(type, onPickup == null ? delegate(){} : onPickup);
        
        item.transform.position = position;

        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        float rnd1 = Random.Range(0, rb.mass)*5;
        float rnd2 = Random.Range(-rb.mass, rb.mass)*2;
        rb.AddForce(new Vector2(rnd1/rb.mass, rnd2/rb.mass));
        rb.AddTorque(rnd1+rnd2);
    }

    public static void Mail(string id, bool mute = false){
        InventoryManager IM = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
        GameObject soundController = GameObject.Find("Sounds");
        if(!mute)
            soundController.SendMessage("playSound", new SoundParameter("MessageReceivedSound", GameObject.FindGameObjectWithTag("Player"), 0.7f, false), SendMessageOptions.DontRequireReceiver);
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

    public static bool gravityIsEnabled(){
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().gravityScale > 0;
    }

    public static void Gravity(bool enable){
        Collider2D ASC = GameObject.FindGameObjectWithTag("AbandonedSpaceshipCollider").GetComponent<PolygonCollider2D>();
        DataPersistenceManager dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        foreach(Rigidbody2D rb in GameObject.FindObjectsOfType<Rigidbody2D>()){
            Collider2D obj_collider = rb.gameObject.GetComponent<Collider2D>();
            string obj_tag = rb.gameObject.tag;
            
            if(obj_collider.IsTouching(ASC)){
                if(obj_tag != "Interactable" && obj_tag != "Stationary"){//object selection criteria
                    rb.gravityScale = enable ? 1 : 0;

                    if(enable){
                        rb.AddForce(new Vector2(0, -10*rb.mass));
                    }

                    //slightly lift the player/loose objects if gravity is turned off
                    //if((!enable) && ((rb.gameObject.tag == "Player") || (rb.tag == "Collectable"))){
                    if(!enable){
                        float lift_force = 100;
                        float rnd = Random.Range(0, lift_force/20);
                        rb.AddForce(new Vector2(0, (lift_force+rnd)/rb.mass));
                        rb.AddTorque(Random.Range(-lift_force/20, lift_force/20));
                    }
                }
            }
        }
        dpm.getGameData().AS_hasGravity = enable;
    }
}
