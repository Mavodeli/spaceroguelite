using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private GameObject player;
    private InventoryManager IM;

    private int arrowsToCollect = 5;
    
    private void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        IM = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();

        triggerMap.Add("PH black hole", delegate () {
            //kill player upon entering the black hole
            player.SendMessage("addHealth", -Mathf.Infinity, SendMessageOptions.DontRequireReceiver);
        });
        triggerMap.Add("TriggerElectroAsteroidsEnemySpawner", delegate () {
            if(!PT.getFlag("triggeredEnemySpawner")){
                int count = 3;
                for(int i = 0; i < count; i++){
                    Vector2 playerLastDirection = player.GetComponent<PlayerMovement>().getMovement();
                    //offset still kinda WIP
                    Vector3 spawnOffsetToPlayer = new Vector3(playerLastDirection.x*12, playerLastDirection.y*12, 0);
                    Vector3 fishToFishOffset = new Vector3(2*i, -2*i, 0);
                    Vector3 position = player.transform.position + spawnOffsetToPlayer + fishToFishOffset;
                    Spawn.Enemy("PufferFish", position);
                }
                PT.setFlag("triggeredEnemySpawner");
            }
        });

        //fill the triggers in the scene with their behaviours according to the trigger map
        foreach(GameObject trigger in GameObject.FindGameObjectsWithTag("ProgressionTrigger")){
            ProgressionTrigger pt = trigger.GetComponent<ProgressionTrigger>();
            pt.Setup(triggerMap[trigger.name]);
        }

        int count = 10;
        for(int i = 0; i < count; i++){
            Spawn.Item("arrow of doom", new Vector3(2*i, -2*i, 0));
        }
    }
    
    void Update()
    {
        if(!PT.getFlag("arrowMessageShown") && IM.ItemAmountInDict("arrow of doom") >= arrowsToCollect){
            Debug.Log("Congrats on collectiong 5 arrows of doom!");
            PT.setFlag("arrowMessageShown");
        }
    }

    public void printPTDict(){
        string str = "FlagDict: [";
        Dictionary<string, bool> dict = PT.getFlagDict();
        foreach(KeyValuePair<string, bool> entry in dict){
            str += "'"+entry.Key+"': "+entry.Value+", ";
        }
        Debug.Log(str+"]");
    }
}
