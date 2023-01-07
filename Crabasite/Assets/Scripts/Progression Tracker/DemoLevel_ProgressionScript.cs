using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT = ProgressionTracker;


public class DemoLevel_ProgressionScript : ProgressionDelegate
{
    private Dictionary<string, Function> triggerMap = new Dictionary<string, Function>();
    private TimerObject timer;


    private void Awake(){
        PT.initProgressionTracker();//since the PT is persistent, it has to be reset at the start of the first level upon beginning a new game!

        triggerMap.Add("TriggerOrangeSwirl", delegate(){
            Debug.Log("Player reached Trigger 'TriggerOrangeSwirl', yay!");
            PT.setFlag("triggeredOrangeSwirl");
        });
        triggerMap.Add("TriggerWhiteOrb", delegate(){
            if(PT.getFlag("triggeredOrangeSwirl") && !PT.getFlag("triggeredWhiteOrb")){
                Debug.Log("Player also reached Trigger 'TriggerWhiteOrb', double yay!");
                PT.setFlag("triggeredWhiteOrb", true);
            }
        });
        timer = new TimerObject(autoDestroy: true);
        timer.start(10);
        PT.setFlag("timerRanOut", false);
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
        if(!timer.runs() && !PT.getFlag("timerRanOut")){
            Debug.Log("10sec Timer ran out!");
            PT.setFlag("timerRanOut", true);
        }
    }
}
