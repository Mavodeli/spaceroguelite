using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestJournal;


public class SpaceLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private QuestJournal QJ;
    
    private void Start() {
        Invoke("delayedStart", 0.2f); // delay start to allow CommentarySystem to fully set up
    }

    private void delayedStart() {
        QJ = gameObject.GetComponent<QuestJournal>();

        triggerMap.Add("black hole", delegate () {
            //kill player upon entering the black hole
            player.SendMessage("addHealth", -99999999, SendMessageOptions.DontRequireReceiver);//creates only problems when using infinity
        });
        triggerMap.Add("TriggerElectroAsteroidsEnemySpawner", delegate () {
            if(!PT.getFlag(enemySpawnPrefix+"SpaceLevelPufferFishies")){
                Spawn.Enemy("PufferFish", new Vector3(8.47f,40.35f,0));
                Spawn.Enemy("PufferFish", new Vector3(-0.88f,38.27f,0));
                Spawn.Enemy("PufferFish", new Vector3(9.72f,32.91f,0));
                Spawn.Enemy("PufferFish", new Vector3(12.5f,42.66f,0));
                Spawn.Enemy("PufferFish", new Vector3(-1.52f,35.95f,0));
                PT.setFlag(enemySpawnPrefix+"SpaceLevelPufferFishies");
            }
        });
        triggerMap.Add("SilicateTrigger", delegate () {
            if(!PT.getFlag("triggeredSilicateTrigger")){
                if((player.GetComponent<UltimateHolder>().ultimate).GetType().ToString() == "AttractTwo"){
                    CommentarySystem.displayProtagonistComment("firstTimeSeeingSilicateAsteroidWithUltimate");
                }
                else{
                    CommentarySystem.displayProtagonistComment("firstTimeSeeingSilicateAsteroidWithoutUltimate");//maybe remove for the sake of better game design?
                    if(!QJ.questIsCompletedOrActive("GetAttractTwo"))
                        QJ.addNewQuest("GetAttractTwo");
                }
                PT.setFlag("triggeredSilicateTrigger");
            }
        });
        triggerMap.Add("ElectroParticleTrigger", delegate () {
            if(!PT.getFlag("triggeredElectroParticleTrigger")){
                CommentarySystem.displayProtagonistComment("firstTimeSeeingElectroAsteroid");
                PT.setFlag("triggeredElectroParticleTrigger");
            }
        });

        triggerMap.Add("AttractTwoAsteroidTrigger", delegate () {
            if(!PT.getFlag("triggeredAttractTwoAsteroidTrigger")){
                CommentarySystem.displayProtagonistComment("firstTimeSeeingAttractTwoAsteroid");
                PT.setFlag("triggeredAttractTwoAsteroidTrigger");
            }
        });

        //fill the triggers in the scene with their behaviours according to the trigger map
        foreach(GameObject trigger in GameObject.FindGameObjectsWithTag("ProgressionTrigger")){
            ProgressionTrigger pt = trigger.GetComponent<ProgressionTrigger>();
            pt.Setup(triggerMap[trigger.name]);
        }

        if(!PT.getFlag("FirstTimeLeavingSpaceship")){
            CommentarySystem.displayProtagonistComment("firstTimeLeavingSpaceship");
            CommentarySystem.displayProtagonistComment("firstTimeLeavingSpaceshipContinued");
            CommentarySystem.displayProtagonistComment("firstTimeSeeingSpaceshipParts");
            PT.setFlag("FirstTimeLeavingSpaceship");
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
