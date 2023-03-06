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
                        SceneManager.LoadScene("Level 1 - space");
                        Time.timeScale = 1;
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

                            //spawn Quest-related items
                            Spawn.Item("Silicone", new Vector3(-27.5743427f,11.0001459f,0.0f), "Level 1 - space");
                            Spawn.Item("Silicone", new Vector3(-39.5380859f,5.4468565f,0.0f), "Level 1 - space");
                            Spawn.Item("Silicone", new Vector3(-21.5161381f,4.44077635f,0.0f), "Level 1 - space");
                            Spawn.Item("Silicone", new Vector3(-36.1542168f,22.2319221f,0.0f), "Level 1 - space");
                            Spawn.Item("Silicone", new Vector3(-36.1542168f,28.5499992f,0.0f), "Level 1 - space");
                        }

                        //fire event interactedWithWindshieldw
                        fireEvent("interactedWithWindshield");
                        
                    }, "e", newShowDistanceMaximum+.2f);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Hyperdrive"){
                    script.Setup(delegate () {
                        //TODO
                        Debug.Log("not implemented yet");
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
                        //TODO
                        Debug.Log("not implemented yet");
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
