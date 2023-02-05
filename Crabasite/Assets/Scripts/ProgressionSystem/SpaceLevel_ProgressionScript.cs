using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private GameObject player;


    private void Start(){
        player = GameObject.FindGameObjectWithTag("Player");

        triggerMap.Add("PH black hole", delegate () {
            //kill player upon entering the black hole
            player.SendMessage("addHealth", -Mathf.Infinity, SendMessageOptions.DontRequireReceiver);
        });
        triggerMap.Add("TriggerElectroAsteroidsEnemySpawner", delegate () {
            if(!PT.getFlag("triggeredEnemySpawner")){
                int count = 3;
                for(int i = 0; i < count; i++){
                    GameObject fish = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
                    fish.AddComponent<PufferFishBehaviour>();
                    Vector2 playerLastDirection = player.GetComponent<PlayerMovement>().getMovement();
                    //offset still kinda WIP
                    Vector3 spawnOffsetToPlayer = new Vector3(playerLastDirection.x*12, playerLastDirection.y*12, 0);
                    Vector3 fishToFishOffset = new Vector3(2*i, -2*i, 0);
                    fish.transform.position = player.transform.position + spawnOffsetToPlayer + fishToFishOffset;
                }
                PT.setFlag("triggeredEnemySpawner");
            }
        });

        //fill the triggers in the scene with their behaviours according to the trigger map
        foreach(GameObject trigger in GameObject.FindGameObjectsWithTag("ProgressionTrigger")){
            ProgressionTrigger pt = trigger.GetComponent<ProgressionTrigger>();
            pt.Setup(triggerMap[trigger.name]);
        }
    }
    
    void Update()
    {
        
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