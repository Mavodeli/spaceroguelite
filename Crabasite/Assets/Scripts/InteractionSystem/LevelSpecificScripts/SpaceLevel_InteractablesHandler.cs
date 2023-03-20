using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceLevel_InteractablesHandler : MonoBehaviour
{
<<<<<<< HEAD:Crabasite/Assets/Scripts/Misc/InteractablesHandler.cs

    void Start(){

    }

=======
>>>>>>> 3d6c4a77a7ba9483e8735b7400b6b2fc348c0e83:Crabasite/Assets/Scripts/InteractionSystem/LevelSpecificScripts/SpaceLevel_InteractablesHandler.cs
    void Update()
    {
        //check if all interactables have the button script, if not, add missing scripts
        foreach(GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable")){
            InteractionButton script = interactable.GetComponent<InteractionButton>();
            if(script == null){
                script = interactable.AddComponent<InteractionButton>();

                if(interactable.name == "SpaceshipEntrance"){
                    script.Setup(delegate () {
<<<<<<< HEAD:Crabasite/Assets/Scripts/Misc/InteractablesHandler.cs
                        SceneManager.LoadScene("SampleScene");//TODO: set correct scene ;)
=======
                        SceneManager.LoadScene("Level 0 - spaceship");
>>>>>>> 3d6c4a77a7ba9483e8735b7400b6b2fc348c0e83:Crabasite/Assets/Scripts/InteractionSystem/LevelSpecificScripts/SpaceLevel_InteractablesHandler.cs
                        Time.timeScale = 1;
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "blue blobby mass circle"){
                    script.Setup(delegate () {
                        int ult = 0;//attract two
                        GameObject IM = GameObject.FindGameObjectWithTag("Inventory");
                        IM.SendMessage("unlockUltimate", ult, SendMessageOptions.DontRequireReceiver);
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        player.SendMessage("SwitchUltimate", ult, SendMessageOptions.DontRequireReceiver);
<<<<<<< HEAD:Crabasite/Assets/Scripts/Misc/InteractablesHandler.cs
                        GameObject IM = GameObject.FindGameObjectWithTag("Inventory");
                        IM.SendMessage("unlockUltimate", ult, SendMessageOptions.DontRequireReceiver);//TODO: not implemented yet!!!
=======
>>>>>>> 3d6c4a77a7ba9483e8735b7400b6b2fc348c0e83:Crabasite/Assets/Scripts/InteractionSystem/LevelSpecificScripts/SpaceLevel_InteractablesHandler.cs
                        Destroy(GameObject.Find(interactable.name));//kinda tricky to get the delegate to destroy the correct gameObject ;)
                    }, "e");
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                //add more interactables here simply by using if checks
            }
        }
    }
}
