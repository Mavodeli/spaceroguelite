using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceshipLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();


    private void Start() {
        Invoke("delayedStart", 0.2f); // delay start to allow CommentarySystem to fully set up
    }

    private void delayedStart() {
        //1st parameter: the name of the GameObject (the 'trigger' object with the ProgressionTrigger script)
        //2nd parameter: the function that should be executed OnTriggerEnter
        triggerMap.Add("BobTheTrigger", delegate () {

        });
      
        //fill the triggers in the scene with their behaviours according to the trigger map
        foreach(GameObject trigger in GameObject.FindGameObjectsWithTag("ProgressionTrigger")){
            ProgressionTrigger pt = trigger.GetComponent<ProgressionTrigger>();
            pt.Setup(triggerMap[trigger.name]);
        }

        if(!PT.getFlag("ProtagonistWokeUp")){
            CommentarySystem.displayProtagonistComment("protagonistWakesUp");
            Spawn.Mail("MissionBriefing", true);
            Spawn.Mail("MissionDetails", true);
            Spawn.Mail("WifeMail0", true);
            PT.setFlag("ProtagonistWokeUp");
        }
    }

    public bool getFlag(string id){
        return PT.getFlag(id);
    }

    public void setFlag(string id, bool value = true){
        PT.setFlag(id, value);
    }
}
