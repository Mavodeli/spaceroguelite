using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractablesHandler : MonoBehaviour
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
                        SceneManager.LoadScene("SampleScene");//TODO: set correct scene ;)
                        Time.timeScale = 1;
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "blue blobby mass circle"){
                    script.Setup(delegate () {
                        int ult = 0;
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        player.SendMessage("SwitchUltimate", ult, SendMessageOptions.DontRequireReceiver);
                        // GameObject hud = GameObject.FindGameObjectWithTag("HUD");
                        // hud.SendMessage("ChangeSprite", ult, SendMessageOptions.DontRequireReceiver);
                        GameObject IM = GameObject.FindGameObjectWithTag("Inventory");
                        IM.SendMessage("unlockUltimate", ult, SendMessageOptions.DontRequireReceiver);//TODO: not implemented yet!!!
                        Destroy(GameObject.Find(interactable.name));//kinda tricky to get the delegate to destroy the correct gameObject ;)
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                //add more interactables here simply by using if checks
            }
        }
    }
}
