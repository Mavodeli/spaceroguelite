using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SpaceLevel_ProgressionScript : ProgressionParentClass
{
    private Dictionary<string, OnTriggerEnterDelegate> triggerMap = new Dictionary<string, OnTriggerEnterDelegate>();
    private GameObject player;


    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player");

        //TODO: workaround until GameData resetting is implemented!
        PT.setFlag("triggeredEnemySpawner", false);

        triggerMap.Add("PH black hole", delegate () {
            //kill player upon entering the black hole
            player.SendMessage("addHealth", -Mathf.Infinity, SendMessageOptions.DontRequireReceiver);
        });
        triggerMap.Add("TriggerSpaceshipEntrance", delegate () {
            SceneManager.LoadScene("SampleScene");//TODO: set correct scene ;)
            Time.timeScale = 1;
            //TODO: show button instead of instant teleport!!!
        });
        triggerMap.Add("TriggerElectroAsteroidsEnemySpawner", delegate () {
            if(!PT.getFlag("triggeredEnemySpawner")){
                int count = 3;
                for(int i = 0; i < count; i++){
                    GameObject fish = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
                    fish.AddComponent<PufferFishBehaviour>();
                    Vector3 spawnOffset = new Vector3(0, 0, 0);
                    fish.transform.position = player.transform.position + spawnOffset;
                }
                PT.setFlag("triggeredEnemySpawner");
            }
        });
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
        
    }
}
