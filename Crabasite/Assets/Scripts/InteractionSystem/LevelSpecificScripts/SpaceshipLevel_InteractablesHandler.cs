using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipLevel_InteractablesHandler : MonoBehaviour
{
    void Update()
    {
        //check if all interactables have the button script, if not, add missing scripts
        foreach(GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable")){
            InteractionButton script = interactable.GetComponent<InteractionButton>();
            if(script == null){
                script = interactable.AddComponent<InteractionButton>();
                float newShowDistanceMaximum = .8f;

                if(interactable.name == "SpaceshipEntrance"){
                    script.Setup(delegate () {

                        if(//ensure that the player 'collected' all quests for the space level
                            QuestIsCompletedOrActive("RepairWindshield") &&
                            QuestIsCompletedOrActive("RepairSpaceship") &&
                            QuestIsCompletedOrActive("RechargeThrusters")
                        ){
                            SceneManager.LoadScene("Level 1 - space");
                            Time.timeScale = 1;
                        }
                        else{
                            CommentarySystem.displayComment("PlayerShouldInspectAllBrokenShipParts");
                        }
                    }, "e", newShowDistanceMaximum+.5f);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Windshield"){
                    script.Setup(delegate () {

                        if(!QuestIsCompletedOrActive("RepairWindshield")){

                            //Quest
                            Spawn.Quest("RepairWindshield");

                            //Comment
                            CommentarySystem.displayComment("startRepairWindshield");
                        }

                        //fire event interactedWithWindshield
                        fireEvent("interactedWithWindshield");
                        
                    }, "e", newShowDistanceMaximum+.2f);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Hyperdrive"){
                    script.Setup(delegate () {
                        
                        if(!QuestIsCompletedOrActive("RechargeThrusters")){

                            //Quest
                            Spawn.Quest("RechargeThrusters");

                            //Comment
                            CommentarySystem.displayComment("startRechargeThrusters");
                        }

                        //fire event interactedWithWindshield
                        fireEvent("interactedWithHyperdrive");

                    }, "e", newShowDistanceMaximum);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Computer"){
                    script.Setup(delegate () {
                        GameObject dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager");
                        dpm.SendMessage("SaveGame", true, SendMessageOptions.DontRequireReceiver);
                        // Debug.Log("Game saved to file.");
                        dpm.SendMessage("LoadGame", true, SendMessageOptions.DontRequireReceiver);
                        // Debug.Log("Game loaded from file.");
                    }, "e", newShowDistanceMaximum);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Workbench"){
                    script.Setup(delegate () {
                        
                        if(!QuestIsCompletedOrActive("RepairSpaceship")){

                            //Quest
                            Spawn.Quest("RepairSpaceship");

                            //Comment
                            CommentarySystem.displayComment("startRepairSpaceship");
                        }

                        //fire event interactedWithWorkbench
                        fireEvent("interactedWithWorkbench");

                    }, "e", newShowDistanceMaximum);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Bed"){
                    script.Setup(delegate () {
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        player.SendMessage("addHealth", Mathf.Infinity, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("Health restored.");
                    }, "e", newShowDistanceMaximum);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }
            }
        }
    }

    private bool QuestIsCompletedOrActive(string id){
        return GameObject.FindGameObjectWithTag("GameHandler").GetComponent<QuestJournal>().questIsCompletedOrActive(id);
    }

    private void fireEvent(string id){
        GameObject.FindGameObjectWithTag("QuestEventsContainer").SendMessage("InvokeEvent", id, SendMessageOptions.DontRequireReceiver);
    }
}
