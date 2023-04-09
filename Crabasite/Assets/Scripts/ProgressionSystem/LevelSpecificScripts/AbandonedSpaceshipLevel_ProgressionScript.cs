using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbandonedSpaceshipLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private DataPersistenceManager dpm;


    private void Start() {
        Invoke("delayedStart", 0.2f); // delay start to allow CommentarySystem to fully set up
    }

    private void delayedStart() {
        dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        //1st parameter: the name of the GameObject (the 'trigger' object with the ProgressionTrigger script)
        //2nd parameter: the function that should be executed OnTriggerEnter
        triggerMap.Add("AS_MantisShrimpTrigger", delegate () {
            if(!PT.getFlag(enemySpawnPrefix+"MantisShrimp")){
                Spawn.Enemy("MantisShrimp", new Vector3(50.0f, 25.72f, 0));
                PT.setFlag(enemySpawnPrefix+"MantisShrimp");
            }
        });

        triggerMap.Add("AS_MorayEelTrigger", delegate () {
            if(!PT.getFlag(enemySpawnPrefix+"MorayEels")){
                Spawn.Enemy("MorayEel", new Vector3(-27.08f, 31.21f, 0));
                Spawn.Enemy("MorayEel", new Vector3(-40.52f, 32.94f, 0));
                Spawn.Enemy("MorayEel", new Vector3(-35.53f, 24, 0));
                PT.setFlag(enemySpawnPrefix+"MorayEels");
            }
        });

        triggerMap.Add("AS_EngineRoomTrigger", delegate () {
            if(!PT.getFlag("firstTimeEnteredEngineRoom")){
                CommentarySystem.displayProtagonistComment("firstTimeEnteringEngineRoomMC1");
                CommentarySystem.displayAIComment("firstTimeEnteringEngineRoomAI");
                CommentarySystem.displayProtagonistComment("firstTimeEnteringEngineRoomMC2");
                Spawn.Mail("HyperdriveReplacement");
                PT.setFlag("firstTimeEnteredEngineRoom");
            }
        });

        triggerMap.Add("AS_LaboratoryTrigger", delegate () {
            if(!PT.getFlag("firstTimeEnteredLaboratory")){
                CommentarySystem.displayAIComment("firstTimeEnteringLaboratoryAI");
                CommentarySystem.displayProtagonistComment("firstTimeEnteringLaboratoryMC1");
                CommentarySystem.displayProtagonistComment("firstTimeEnteringLaboratoryMC2");
                Spawn.Mail("WifeMail3");
                PT.setFlag("firstTimeEnteredLaboratory");
            }
        });

        triggerMap.Add("AS_EnterSpaceshipTrigger", delegate () {
            if(!PT.getFlag("firstTimeEnteredAbandonedSpaceship")){
                CommentarySystem.displayProtagonistComment("firstTimeEnteringAbandonedSpaceshipMC1");
                CommentarySystem.displayAIComment("firstTimeEnteringAbandonedSpaceshipAI");
                CommentarySystem.displayProtagonistComment("firstTimeEnteringAbandonedSpaceshipMC2");
                PT.setFlag("firstTimeEnteredAbandonedSpaceship");
            }
        });

        triggerMap.Add("AS_EngineRoomHatchTrigger", delegate () {
            if(!PT.getFlag("firstTimeSeenEngineRoomHatch")){
                CommentarySystem.displayProtagonistComment("startOpenHatch");
                Spawn.Quest("OpenHatch");
                PT.setFlag("firstTimeSeenEngineRoomHatch");
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

        if(!PT.getFlag("AbandonedSpaceshipMailDumpHappened")){
            Spawn.Mail("MissionReportHQ");
            Spawn.Mail("WifeMail1.5", true);
            Spawn.Mail("WifeMail1", true);
            Spawn.Mail("MissionReport", true);
            Spawn.Mail("WifeMail2", true);
            PT.setFlag("AbandonedSpaceshipMailDumpHappened");
        }

        //spawn seelie if player doesn't have neg. charge unlocked
        if(!dpm.getGameData().UltimateDict[2])
            Spawn.Seelie(new Vector3(-24.8f,6.6f,0), true);
    }

    public bool getFlag(string id){
        return PT.getFlag(id);
    }

    public void setFlag(string id, bool value = true){
        PT.setFlag(id, value);
    }
}
