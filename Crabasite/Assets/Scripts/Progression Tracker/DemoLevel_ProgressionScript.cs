using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DemoLevel_ProgressionScript : ProgressionDelegate
{
    private Dictionary<string, Function> triggerMap = new Dictionary<string, Function>();
    private TimerObject timer;


    private void Awake(){
        triggerMap.Add("TriggerOrangeSwirl", delegate(){
            Debug.Log("Player reached Trigger 'TriggerOrangeSwirl', yay!");
            ProgressionTracker.setFlag("triggeredOrangeSwirl");
        });
        triggerMap.Add("TriggerWhiteOrb", delegate(){
            if(ProgressionTracker.isTrueAt("triggeredOrangeSwirl")){
                Debug.Log("Player also reached Trigger 'TriggerWhiteOrb', double yay!");
                ProgressionTracker.setFlag("triggeredWhiteOrb", true);
            }
        });
        timer = new TimerObject(autoDestroy: true);
        timer.start(10);
        ProgressionTracker.setFlag("timerRanOut", false);
    }

    void Start()
    {   
        //fill the triggers in the scene with their behaviours according to the trigger map
        foreach(GameObject trigger in GameObject.FindGameObjectsWithTag("ProgressionTrigger")){
            ProgressionTrigger pt = trigger.GetComponent<ProgressionTrigger>();
            pt.Setup(triggerMap[trigger.name]);
        }
    }

    
    void Update()
    {
        if(!timer.runs() && !ProgressionTracker.isTrueAt("timerRanOut")){
            Debug.Log("10sec Timer ran out!");
            ProgressionTracker.setFlag("timerRanOut", true);
        }

    }
}
