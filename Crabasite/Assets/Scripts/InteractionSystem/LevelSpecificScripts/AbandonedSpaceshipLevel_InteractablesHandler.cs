using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbandonedSpaceshipLevel_InteractablesHandler : MonoBehaviour
{
    void Start()
    {
        //check if all interactables have the button script, if not, add missing scripts
        foreach(GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable")){
            InteractionButton script = interactable.GetComponent<InteractionButton>();
            if(script == null){
                script = interactable.AddComponent<InteractionButton>();

                if(interactable.name == "SpaceshipEntrance"){
                    script.Setup(delegate () {
                        SceneManager.LoadScene("Level 0 - spaceship");
                        Time.timeScale = 1;
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "GravitySwitch"){
                    script.Setup(delegate () {
                        Spawn.Gravity(!Spawn.gravityIsEnabled());
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "SecretRoomInteractable"){
                    script.Setup(delegate () {
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC1.1");
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC1.2");
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC1.3");
                        CommentarySystem.displayProtagonistComment("cuttingRoomFloorMC1");
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC2");
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC3");
                        CommentarySystem.displayAIComment("cuttingRoomFloorAI1");
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC4.1");
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC4.2");
                        CommentarySystem.displayAIComment("cuttingRoomFloorAI2");
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC5");
                        CommentarySystem.displayProtagonistComment("cuttingRoomFloorMC2");
                        CommentarySystem.displayBCComment("cuttingRoomFloorBC6");
                        CommentarySystem.displayProtagonistComment("cuttingRoomFloorMC3");
                    }, "e", 2);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }
            }
        }

        //set initial gravity
        DataPersistenceManager dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        Spawn.Gravity(dpm.getGameData().AS_hasGravity);
    }
}
