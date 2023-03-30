using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbandonedSpaceshipLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private DataPersistenceManager dpm;


    private void Start(){
        dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        //1st parameter: the name of the GameObject (the 'trigger' object with the ProgressionTrigger script)
        //2nd parameter: the function that should be executed OnTriggerEnter
        triggerMap.Add("AS_MantisShrimpTrigger", delegate () {
            if(!PT.getFlag(enemySpawnPrefix+"MantisShrimp")){
                Spawn.Enemy("MantisShrimp", new Vector3(55.88f, 25.72f, 0));//TODO new sprites not implemented yet!!!
                PT.setFlag(enemySpawnPrefix+"MantisShrimp");
            }
        });

        triggerMap.Add("AS_EngineRoomTrigger", delegate () {
            if(!PT.getFlag("firstTimeEnteredEngineRoom")){
                
                PT.setFlag("firstTimeEnteredEngineRoom");
            }
        });

        triggerMap.Add("AS_LaboratoryTrigger", delegate () {
            if(!PT.getFlag("firstTimeEnteredLaboratory")){
                
                PT.setFlag("firstTimeEnteredLaboratory");
            }
        });

        triggerMap.Add("AS_EnterSpaceshipTrigger", delegate () {
            if(!PT.getFlag("firstTimeEnteredAbandonedSpaceship")){
                
                PT.setFlag("firstTimeEnteredAbandonedSpaceship");
            }
        });
      
        //fill the triggers in the scene with their behaviours according to the trigger map
        foreach(GameObject trigger in GameObject.FindGameObjectsWithTag("ProgressionTrigger")){
            ProgressionTrigger pt = trigger.GetComponent<ProgressionTrigger>();
            pt.Setup(triggerMap[trigger.name]);
        }

        if(!PT.getFlag(enemySpawnPrefix+"Anglerfish")){
            Vector3 position = new Vector3(0.170000002f, 9.89999962f, 0.0f);

            if(!dpm.getGameData().UltimateDict[1])//if player doesn't have ult
                Spawn.Enemy("AnglerFish", position, new string[]{"CrushOrb"});
            else
                Spawn.Enemy("AnglerFish", position);

            PT.setFlag(enemySpawnPrefix+"Anglerfish");
        }
    }

    public bool getFlag(string id){
        return PT.getFlag(id);
    }

    public void setFlag(string id, bool value = true){
        PT.setFlag(id, value);
    }
}
