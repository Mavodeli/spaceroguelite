using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

            string quest_identifier = "RepairWindshield";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithWindshield",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("Silicone") >= 12;//maybeTODO: update amount
                    },
                    delegate(){//onCompletion
                        Debug.Log("Repairing windshield...");
                        Spawn.NewSprite("Spaceship_patchedWindshield", GameObject.FindGameObjectWithTag("ShipHull"));//TODO: change to actual sprite name!!!
                        CommentarySystem.displayComment("completedRepairWindshield");//maybeTODO: use correct identifier
                })
            );

            quest_identifier = "GetAttractTwo";
            //TODO (get this quest when near the silicone asteroids)

            quest_identifier = "RechargeThrusters";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithHyperdrive",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("ElectroParticle") >= 8;//maybeTODO: update amount
                    },
                    delegate(){//onCompletion
                        Debug.Log("Recharging Thrusters...");
                        CommentarySystem.displayComment("completedRechargeThrusters");//maybeTODO: use correct identifier
                })
            );

            quest_identifier = "RepairSpaceship";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "interactedWithWorkbench",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("SpaceshipDebris") >= 8;//maybeTODO: update amount
                    },
                    delegate(){//onCompletion
                        Debug.Log("Repairing spaceship...");
                        Spawn.NewSprite("Spaceship_repaired", GameObject.FindGameObjectWithTag("ShipHull"));//TODO: change to actual sprite name!!!
                        CommentarySystem.displayComment("completedRepairSpaceship");//maybeTODO: use correct identifier
                })
            );

            quest_identifier = "new Quest";
            //TODO
        }

        public Quest at(string key){ return data[key];}
    }
}
