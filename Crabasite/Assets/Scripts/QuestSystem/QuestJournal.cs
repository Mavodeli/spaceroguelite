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

    public void addNewQuest(string identifier, bool debug_mode = false){
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

            string quest_identifier = "collect_five_arrows";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,
                    "moveItemToInventory",
                    delegate(){//completionCriterion
                        return IM.ItemAmountInDict("arrow of doom") >= 5;
                    },
                    delegate(){//onCompletion
                        Debug.Log("Congrats on collectiong 5 arrows of doom!");
                })
            );

            quest_identifier = "new Quest";
            //TODO
        }

        public Quest at(string key){ return data[key];}
    }
}
