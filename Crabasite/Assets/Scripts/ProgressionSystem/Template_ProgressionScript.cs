using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Template_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();


    private void Start(){
        //1st parameter: the name of the GameObject (the 'trigger' object with the ProgressionTrigger script)
        //2nd parameter: the function that should be executed OnTriggerEnter
        triggerMap.Add("BobTheTrigger", delegate () {
            //TODO
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
}
