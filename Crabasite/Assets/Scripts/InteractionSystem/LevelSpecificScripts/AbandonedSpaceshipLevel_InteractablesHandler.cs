using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbandonedSpaceshipLevel_InteractablesHandler : MonoBehaviour
{
    void Update()
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

                // if(interactable.name == "GetCrush"){
                //     script.Setup(delegate () {
                //         int ult = 1;//crush
                //         GameObject IM = GameObject.FindGameObjectWithTag("Inventory");
                //         IM.SendMessage("unlockUltimate", ult, SendMessageOptions.DontRequireReceiver);
                //         GameObject player = GameObject.FindGameObjectWithTag("Player");
                //         player.SendMessage("SwitchUltimate", ult, SendMessageOptions.DontRequireReceiver);
                //         Destroy(GameObject.Find(interactable.name));
                //     }, "e");
                //     script.setNewOffset(new Vector3(0, 0, 0));
                // }

                if(interactable.name == "GravitySwitch"){
                    script.Setup(delegate () {
                        Spawn.Gravity(!Spawn.gravityIsEnabled());
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }
            }
        }
    }
}
