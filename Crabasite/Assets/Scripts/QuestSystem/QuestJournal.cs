using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class QuestJournal : MonoBehaviour, IDataPersistence
{   
    private Dictionary<string, bool> activeQuests = new Dictionary<string, bool>();
    //true if quest active, false if completed, not in dict if never started
    private QuestGlossary questGlossary;
    private QuestEvents questEvents;
    private bool debug_mode = false;

    public void updateStatusForCompletedQuest(string quest_identifier){
        try
        {
            activeQuests[quest_identifier] = false;
        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning("The quest "+quest_identifier+" is not an active Quest!");
        }
    }

    public void addNewQuest(string identifier){
        //check the status of the Quest (is it active or completed?)
        try
        {
            if(activeQuests[identifier]){
                if(debug_mode) Debug.LogWarning("Tried to add the Quest "+identifier+" which already existed in activeQuests. The new Quest was not added.");
                return;
            }
            else{
                if(debug_mode) Debug.LogWarning("Tried to add the Quest "+identifier+" which has already been completed. The new Quest was not added.");
                return;
            }
        }
        catch (KeyNotFoundException)
        {
            try//check if the Quest exists in the glossary
            {
                questGlossary.at(identifier).activate();
            }
            catch (KeyNotFoundException)
            {
                if(debug_mode)Debug.LogWarning("Tried to add the Quest "+identifier+" which doesn't exist in the Quest Glossary. The new Quest was not added.");
                return;
            }
            //actually add the Quest :)
            activeQuests[identifier] = true;
        }
    }

    public bool questIsCompletedOrActive(string identifier){
        bool b;
        try
        {
            bool tmp = activeQuests[identifier];
            b = true;
        }
        catch (KeyNotFoundException)
        {
            b = false;
        }
        return b;
    }

    public bool questIsCompleted(string identifier){
        bool b;
        try
        {
            b = !activeQuests[identifier];
        }
        catch (KeyNotFoundException)
        {
            b = false;
        }
        return b;
    }

    public void LoadData(GameData data)
    {
        questGlossary = new QuestGlossary(gameObject, GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>());

        activeQuests.Clear();
        foreach(KeyValuePair<string, bool> entry in data.activeQuests){
            activeQuests.Add(entry.Key, entry.Value);
            if(entry.Value) questGlossary.at(entry.Key).activate();
        }
    }
    
    public void SaveData(ref GameData data)
    {
        data.activeQuests.Clear();
        foreach(KeyValuePair<string, bool> entry in activeQuests){
            data.activeQuests.Add(entry.Key, entry.Value);
            questGlossary.at(entry.Key).deactivate();
        }
    }

    private struct QuestGlossary{
        private Dictionary<string, Quest> data;

        public QuestGlossary(
            GameObject GameHandler,
            InventoryManager IM,
            Dictionary<string, Quest> old_glossary = null
        ){
            data = old_glossary == null ? new Dictionary<string, Quest>() : old_glossary;
            QuestJournal QJ = GameHandler.GetComponent<QuestJournal>();
            string quest_identifier;

            quest_identifier = "FindSilicate";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "moveItemToInventory",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("Silicate") >= 8;
                    },
                    delegate(){//onCompletion
                        CommentarySystem.displayProtagonistComment("completedFindSilicate");
                        Spawn.Quest("RefineSilicate");
                })
            );

            quest_identifier = "RefineSilicate";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithHyperdrive",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("Silicate") >= 8;
                    },
                    delegate(){//onCompletion
                        IM.RemoveItem("Silicate", 8);
                        IM.AddItem("Silicone");
                        CommentarySystem.displayProtagonistComment("completedRefineSilicate");
                        Spawn.Quest("RepairWindshield");
                })
            );

            quest_identifier = "RepairWindshield";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithWindshield",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("Silicone") >= 1;
                    },
                    delegate(){//onCompletion
                        IM.RemoveItem("Silicone");
                        CommentarySystem.displayProtagonistComment("completedRepairWindshield");
                        Spawn.NewSprite(ConstructSpriteString.Spaceship(
                            SceneManager.GetActiveScene().name,
                            true,
                            QJ.questIsCompleted("RepairSpaceship"),
                            QJ.questIsCompleted("InstallNewHyperdriveCore")
                        ), GameObject.FindGameObjectWithTag("ShipHull"));
                })
            );

            quest_identifier = "GetAttractTwo";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "unlockUltimate",
                    delegate(){//completionCriterion
                        return IM.ultimateIsUnlocked(0);
                    },
                    delegate(){//onCompletion
                        CommentarySystem.displayProtagonistComment("completedGetAttractTwo");//maybeTODO: use correct identifier
                })
            );

            quest_identifier = "RechargeThrusters";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithHyperdrive",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("ElectroParticle") >= 12;//maybeTODO: update amount
                    },
                    delegate(){//onCompletion
                        IM.RemoveItem("ElectroParticle", 12);
                        CommentarySystem.displayProtagonistComment("completedRechargeThrusters");//maybeTODO: use correct identifier
                })
            );

            quest_identifier = "RepairSpaceship";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithWorkbench",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("SpaceshipDebris") >= 7;//maybeTODO: update amount
                    },
                    delegate(){//onCompletion
                        IM.RemoveItem("SpaceshipDebris", 7);
                        CommentarySystem.displayProtagonistComment("completedRepairSpaceship");
                })
            );

            quest_identifier = "FindANewHyperdriveCore";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "moveItemToInventory",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("HyperdriveCore") >= 1;
                    },
                    delegate(){//onCompletion
                        CommentarySystem.displayProtagonistComment("completedFindANewHyperdriveCore");
                        Spawn.Quest("InstallNewHyperdriveCore");
                })
            );

            quest_identifier = "InstallNewHyperdriveCore";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithHyperdrive",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("HyperdriveCore") >= 1;
                    },
                    delegate(){//onCompletion
                        IM.RemoveItem("HyperdriveCore");
                        Spawn.NewSprite(ConstructSpriteString.Spaceship(
                            SceneManager.GetActiveScene().name,
                            QJ.questIsCompleted("RepairWindshield"),
                            QJ.questIsCompleted("RepairSpaceship"),
                            true
                        ), GameObject.FindGameObjectWithTag("ShipHull"));
                        CommentarySystem.displayProtagonistComment("completedInstallNewHyperdriveCore");
                        if(QJ.questIsCompleted("FindACure"))
                            CommentarySystem.displayProtagonistComment("startTheJourneyHome");
                })
            );

            quest_identifier = "FindACure";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "moveItemToInventory",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("TheCure") >= 1;
                    },
                    delegate(){//onCompletion
                        CommentarySystem.displayProtagonistComment("completedFindACure");
                        Spawn.Mail("WifeMail4");
                        if(QJ.questIsCompleted("InstallNewHyperdriveCore"))
                            CommentarySystem.displayProtagonistComment("startTheJourneyHome");
                })
            );

            quest_identifier = "OpenTheEmergencyDoor";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithEmergencyDoor",
                    delegate(){//completionCriterion
                        return GameObject.FindObjectOfType<EmergencyDoorBehaviour>().isUnlocked();
                    },
                    delegate(){//onCompletion
                        CommentarySystem.displayProtagonistComment("completedOpenTheEmergencyDoor");
                })
            );

            quest_identifier = "OpenHatch";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "detachedEngineRoomHatch",
                    delegate(){//completionCriterion
                        return GameObject.FindObjectOfType<EngineRoomHatchBehaviour>().isLoose();
                    },
                    delegate(){//onCompletion
                        CommentarySystem.displayProtagonistComment("completedOpenHatch");
                })
            );
        }

        public Quest at(string key){ return data[key];}
    }
}
