using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipLevel_InteractablesHandler : MonoBehaviour
{
    private SpaceshipLevel_ProgressionScript progressionScript;

    void Start()
    {
        progressionScript = gameObject.GetComponent<SpaceshipLevel_ProgressionScript>();
        //check if all interactables have the button script, if not, add missing scripts
        foreach(GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable")){
            InteractionButton script = interactable.GetComponent<InteractionButton>();
            if(script == null){
                script = interactable.AddComponent<InteractionButton>();
                float newShowDistanceMaximum = .8f;

                if(interactable.name == "SpaceshipEntrance"){
                    script.Setup(delegate () {
                        showCommentOnInspectingCrabasite();

                        if(true//ensure that the player 'collected' all quests for the space level
                            // QuestIsCompletedOrActive("FindSilicate") &&
                            // QuestIsCompletedOrActive("RepairSpaceship") &&
                            // QuestIsCompletedOrActive("RechargeThrusters")
                        ){
                            GameObject.Find("Sounds").SendMessage("playSound", new SoundParameter("SpaceShipDoor", GameObject.Find("SoundHolder"), 1f, true), SendMessageOptions.DontRequireReceiver);
                            // SceneManager.LoadScene("Level 1 - space");

                            // debug scene switch
                            SceneManager.LoadScene("Level 2 - abandoned spaceship");

                            // debug ulti unlock
                            GameObject inv = GameObject.FindGameObjectWithTag("Inventory");
                            inv.SendMessage("unlockUltimate", 0, SendMessageOptions.DontRequireReceiver);
                            inv.SendMessage("unlockUltimate", 1, SendMessageOptions.DontRequireReceiver);
                            inv.SendMessage("unlockUltimate", 2, SendMessageOptions.DontRequireReceiver);
                            GameObject player = GameObject.FindGameObjectWithTag("Player");
                            player.SendMessage("SwitchUltimate", 0, SendMessageOptions.DontRequireReceiver);

                            Time.timeScale = 1;
                        }
                        else{
                            CommentarySystem.displayProtagonistComment("PlayerShouldInspectAllBrokenShipParts");
                        }
                    }, "e", newShowDistanceMaximum+.5f);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Windshield"){
                    script.Setup(delegate () {
                        showCommentOnInspectingCrabasite();

                        if(!QuestIsCompletedOrActive("FindSilicate")){

                            //Quest
                            Spawn.Quest("FindSilicate");

                            //Comment
                            CommentarySystem.displayProtagonistComment("startFindSilicate");

                            showCommentOnAllQuestsCollected();
                        }

                        //fire event interactedWithWindshield
                        fireEvent("interactedWithWindshield");

                        showCommentOnEmergencyRepairsCompleted();
    
                        bool thrustersCompleted = QuestIsCompleted("RechargeThrusters");
                        bool windshieldCompleted = QuestIsCompleted("RepairWindshield");
                        bool spaceshipCompleted = QuestIsCompleted("RepairSpaceship");
                        bool cureCompleted = QuestIsCompleted("FindACure");
                        bool coreCompleted = QuestIsCompleted("FindANewHyperdriveCore");

                        // Debug.Log(thrustersCompleted);
                        // Debug.Log(windshieldCompleted);
                        // Debug.Log(spaceshipCompleted);
                        // Debug.Log(cureCompleted);
                        // Debug.Log(coreCompleted);

                        if (thrustersCompleted && windshieldCompleted && spaceshipCompleted && !cureCompleted && !coreCompleted)
                        {
                            SceneManager.LoadScene("StoryScene1");
                        } else if (thrustersCompleted && windshieldCompleted && spaceshipCompleted && cureCompleted && coreCompleted)
                        {
                            SceneManager.LoadScene("StoryScene2");
                        }                       

                    }, "e", newShowDistanceMaximum+.2f);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Hyperdrive"){
                    script.Setup(delegate () {
                        showCommentOnInspectingCrabasite();
                        
                        if(!QuestIsCompletedOrActive("RechargeThrusters")){

                            //Quest
                            Spawn.Quest("RechargeThrusters");

                            //Comment
                            CommentarySystem.displayProtagonistComment("startRechargeThrusters");
                        }

                        if(!QuestIsCompletedOrActive("FindANewHyperdriveCore")){
                            Spawn.Quest("FindANewHyperdriveCore");
                            CommentarySystem.displayProtagonistComment("startFindANewHyperdriveCore");

                            showCommentOnAllQuestsCollected();
                        }

                        if(QuestIsCompleted("InstallNewHyperdriveCore")){
                            CommentarySystem.displayProtagonistComment("HyperdriveCoreIsInstalled");
                        }

                        fireEvent("interactedWithHyperdrive");

                        showCommentOnEmergencyRepairsCompleted();

                    }, "e", newShowDistanceMaximum);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Computer"){
                    script.Setup(delegate () {
                        showCommentOnInspectingCrabasite();

                        if(!QuestIsCompletedOrActive("RepairSpaceship")){

                            //Quest
                            Spawn.Quest("RepairSpaceship");

                            //Comment
                            CommentarySystem.displayProtagonistComment("startRepairSpaceship");

                            showCommentOnAllQuestsCollected();
                        }

                        GameObject dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager");
                        dpm.SendMessage("SaveGame", true, SendMessageOptions.DontRequireReceiver);
                        dpm.SendMessage("LoadGame", true, SendMessageOptions.DontRequireReceiver);
                        CommentarySystem.displayProtagonistComment("gameSaved");
                    }, "e", newShowDistanceMaximum);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Workbench"){
                    script.Setup(delegate () {
                        showCommentOnInspectingCrabasite();

                        //fire event interactedWithWorkbench
                        fireEvent("interactedWithWorkbench");

                        showCommentOnEmergencyRepairsCompleted();

                    }, "e", newShowDistanceMaximum);
                    script.setNewOffset(new Vector3(0, 0, 0));
                }

                if(interactable.name == "Bed"){
                    script.Setup(delegate () {
                        showCommentOnInspectingCrabasite();

                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        player.SendMessage("addHealth", 100, SendMessageOptions.DontRequireReceiver);
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

    private bool QuestIsCompleted(string id){
        return GameObject.FindGameObjectWithTag("GameHandler").GetComponent<QuestJournal>().questIsCompleted(id);
    }

    private void fireEvent(string id){
        GameObject.FindGameObjectWithTag("QuestEventsContainer").SendMessage("InvokeEvent", id, SendMessageOptions.DontRequireReceiver);
    }

    private void showCommentOnAllQuestsCollected(){
        if(
            !progressionScript.getFlag("protagonistCollectedAllSpaceshipQuests") &&
            QuestIsCompletedOrActive("FindSilicate") &&
            QuestIsCompletedOrActive("RepairSpaceship") &&
            QuestIsCompletedOrActive("RechargeThrusters")
        ){
            CommentarySystem.displayProtagonistComment("protagonistCollectedAllSpaceshipQuests");
            progressionScript.setFlag("protagonistCollectedAllSpaceshipQuests");
        }
    }

    private void showCommentOnEmergencyRepairsCompleted(){
        if(
            !progressionScript.getFlag("ProtagonistCompletedEmergencyRepairs") &&
            QuestIsCompleted("RepairWindshield") &&
            QuestIsCompleted("RepairSpaceship") &&
            QuestIsCompleted("RechargeThrusters")
        ){
            CommentarySystem.displayProtagonistComment("protagonistCompletedEmergencyRepairs");
            progressionScript.setFlag("ProtagonistCompletedEmergencyRepairs");
        }
    }

    private void showCommentOnInspectingCrabasite(){
        if(!progressionScript.getFlag("protagonistInspectedCrabasite")){
            CommentarySystem.displayProtagonistComment("protagonistInspectsCrabasite");
            CommentarySystem.displayProtagonistComment("startFindACure");
            Spawn.Quest("FindACure");
            progressionScript.setFlag("protagonistInspectedCrabasite");
        }
    }
}
