using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceLevel_InteractablesHandler : MonoBehaviour
{

    private GameObject soundController;

    void Awake(){
        soundController = GameObject.Find("Sounds");
    }

    void Update()
    {
        //check if all interactables have the button script, if not, add missing scripts
        foreach(GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable")){
            InteractionButton script = interactable.GetComponent<InteractionButton>();
            if(script == null){
                script = interactable.AddComponent<InteractionButton>();

                if(interactable.name == "SpaceshipEntrance"){
                    script.Setup(delegate () {
                        //soundController.SendMessage("playSound", new SoundParameter("SpaceShipDoor", GameObject.Find("Player"), 1f, true));
                        // the above line of code duplicates the player
                        SceneManager.LoadScene("Level 0 - spaceship");
                        Time.timeScale = 1;
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "blue blobby mass circle"){
                    script.Setup(delegate () {
                        int ult = 0;//attract two
                        GameObject IM = GameObject.FindGameObjectWithTag("Inventory");
                        IM.SendMessage("unlockUltimate", ult, SendMessageOptions.DontRequireReceiver);
                        soundController.SendMessage("playSound", new SoundParameter("PickupUlt", GameObject.Find("Player"), 1f, false));
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        player.SendMessage("SwitchUltimate", ult, SendMessageOptions.DontRequireReceiver);
                        Destroy(GameObject.Find(interactable.name));//kinda tricky to get the delegate to destroy the correct gameObject ;)
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                //add more interactables here simply by using if checks
            }
        }
    }
}
