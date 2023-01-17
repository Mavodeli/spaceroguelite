using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DemoLevel_ProgressionScript : ProgressionDelegate
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private TimerObject timer;
    private TimerObject orb_timer;


    private void Awake(){
        orb_timer = new TimerObject(autoDestroy: true);

        triggerMap.Add("TriggerOrangeSwirl", delegate () {
            Debug.Log("Player reached Trigger 'TriggerOrangeSwirl', yay!");
            PT.setFlag("triggeredOrangeSwirl");
        });
        triggerMap.Add("TriggerWhiteOrb", delegate(){
            if(PT.getFlag("triggeredOrangeSwirl") && !PT.getFlag("triggeredWhiteOrb")){
                Debug.Log("Player also reached Trigger 'TriggerWhiteOrb', double yay!");
                orb_timer.start(5);
                PT.setFlag("triggeredWhiteOrb", true);
            }
        });
        timer = new TimerObject(autoDestroy: true);
        timer.start(10);
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

        if(PT.getFlag("triggeredWhiteOrb") && !orb_timer.runs() && !PT.getFlag("orbTimerRanOut")){
            Debug.Log("orb Timer ran out!");
            PT.setFlag("orbTimerRanOut", true);
        }

        InventoryManager IM = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
    }
}
