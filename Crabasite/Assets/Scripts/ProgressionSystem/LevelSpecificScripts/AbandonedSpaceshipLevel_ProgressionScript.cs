using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbandonedSpaceshipLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private GameObject player;
    private DataPersistenceManager dpm;


    private void Start(){
        //save player GameObject for easy access
        player = GameObject.FindGameObjectWithTag("Player");
        dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        //1st parameter: the name of the GameObject (the 'trigger' object with the ProgressionTrigger script)
        //2nd parameter: the function that should be executed OnTriggerEnter
        triggerMap.Add("BobTheTrigger", delegate () {

        });
      
        //fill the triggers in the scene with their behaviours according to the trigger map
        foreach(GameObject trigger in GameObject.FindGameObjectsWithTag("ProgressionTrigger")){
            ProgressionTrigger pt = trigger.GetComponent<ProgressionTrigger>();
            pt.Setup(triggerMap[trigger.name]);
        }

        if(!PT.getFlag("AnglerfishSpawned") || !dpm.getGameData().UltimateDict[1]){//spawn if not already spawned or if player doesn't have ult

            Vector3 position = new Vector3(0.170000002f, 9.89999962f, 0.0f);
            GameObject fish = Spawn.Enemy("AnglerFish", position);

            Enemy.OnDeath onDeath = delegate(){
                Spawn.Item("CrushOrb", position, delegate(){
                    int ult = 1;//crush
                    GameObject IM = GameObject.FindGameObjectWithTag("Inventory");
                    IM.SendMessage("unlockUltimate", ult, SendMessageOptions.DontRequireReceiver);
                    player.SendMessage("SwitchUltimate", ult, SendMessageOptions.DontRequireReceiver);
                });
            };
            fish.SendMessage("changeOnDeath", onDeath, SendMessageOptions.DontRequireReceiver);
            
            PT.setFlag("AnglerfishSpawned");
        }
    }

    public bool getFlag(string id){
        return PT.getFlag(id);
    }

    public void setFlag(string id, bool value = true){
        PT.setFlag(id, value);
    }
}
