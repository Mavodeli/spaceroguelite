using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestJournal;


public class SpaceLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private GameObject player;
    private QuestJournal QJ;
    
    private void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        QJ = gameObject.GetComponent<QuestJournal>();

        triggerMap.Add("PH black hole", delegate () {
            //kill player upon entering the black hole
            player.SendMessage("addHealth", -99999999, SendMessageOptions.DontRequireReceiver);//creates only problems when using infinity
        });
        triggerMap.Add("TriggerElectroAsteroidsEnemySpawner", delegate () {
            if(!PT.getFlag("triggeredEnemySpawner")){//maybeTODO: remove flag for dark souls spawning behaviour
                int count = 3;
                for(int i = 0; i < count; i++){
                    Vector2 playerLastDirection = player.GetComponent<PlayerMovement>().getMovement();
                    //offset still kinda WIP
                    Vector3 spawnOffsetToPlayer = new Vector3(playerLastDirection.x*15, playerLastDirection.y*15, 0);
                    Vector3 fishToFishOffset = new Vector3(2*i, -2*i, 0);
                    Vector3 position = player.transform.position + spawnOffsetToPlayer + fishToFishOffset;
                    Spawn.Enemy("PufferFish", position);
                }
                PT.setFlag("triggeredEnemySpawner");
            }
        });
        triggerMap.Add("SilicateTrigger", delegate () {
            if(!PT.getFlag("triggeredSilicateTrigger")){
                if((player.GetComponent<UltimateHolder>().ultimate).GetType().ToString() == "AttractTwo"){
                    CommentarySystem.displayComment("firstTimeSeeingSilicateAsteroidWithUltimate");
                }
                else{
                    CommentarySystem.displayComment("firstTimeSeeingSilicateAsteroidWithoutUltimate");
                    if(!QJ.questIsCompletedOrActive("GetAttractTwo"))
                        QJ.addNewQuest("GetAttractTwo");
                }
                PT.setFlag("triggeredSilicateTrigger");
            }
        });
        triggerMap.Add("ElectroParticleTrigger", delegate () {
            if(!PT.getFlag("triggeredElectroParticleTrigger")){
                CommentarySystem.displayComment("firstTimeSeeingElectroAsteroid");
                PT.setFlag("triggeredElectroParticleTrigger");
            }
        });

        //fill the triggers in the scene with their behaviours according to the trigger map
        foreach(GameObject trigger in GameObject.FindGameObjectsWithTag("ProgressionTrigger")){
            ProgressionTrigger pt = trigger.GetComponent<ProgressionTrigger>();
            pt.Setup(triggerMap[trigger.name]);
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
