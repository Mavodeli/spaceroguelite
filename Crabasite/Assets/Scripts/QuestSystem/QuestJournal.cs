using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestJournal : MonoBehaviour, IDataPersistence
{   
    private Dictionary<string, bool> activeQuests = new Dictionary<string, bool>();
    //true if quest active, false if completed, not in dict if never started
    private QuestGlossary questGlossary;

    private UnityEvent Event_moveItemToInventory;

    private void InvokeEvent_moveItemToInventory(){
        if(Event_moveItemToInventory==null){
            Debug.Log("InvokeEvent_moveItemToInventory: event not found");
        }
        Event_moveItemToInventory?.Invoke();
    }

    private void updateStatusForCompletedQuest(string quest_identifier){
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

        try//check if the Quest exists in the glossary
        {
            questGlossary.at(identifier).activate();
        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning("Tried to add the Quest "+identifier+" which doesn't exist in the Quest Glossary. The new Quest was not added.");
            return;
        }

        //check the status of the Quest (is it active or completed?)
        try
        {
            if(activeQuests[identifier]){
                Debug.LogWarning("Tried to add the Quest "+identifier+" which already existed in activeQuests. The new Quest was not added.");
                return;
            }
            else{
                Debug.LogWarning("Tried to add the Quest "+identifier+" which has already been completed. The new Quest was not added.");
                return;
            }
        }
        catch (KeyNotFoundException)
        {
            //actually add the Quest :)
            activeQuests[identifier] = true;
        }
    }

    public void LoadData(GameData data)
    {
        if(Event_moveItemToInventory==null){
            Debug.Log("LoadData: event not found");
        }
        Event_moveItemToInventory = data.Event_moveItemToInventory;
        Event_moveItemToInventory.RemoveAllListeners();

        questGlossary = new QuestGlossary(gameObject, GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>(),
            // Events
            Event_moveItemToInventory
        );

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
        if(Event_moveItemToInventory==null){
            Debug.Log("SaveData: event not found");
        }
        data.Event_moveItemToInventory = Event_moveItemToInventory;
    }

    private struct QuestGlossary{
        private Dictionary<string, Quest> data;

        public QuestGlossary(
            GameObject GameHandler,
            InventoryManager IM,
            UnityEvent Event_moveItemToInventory,
            Dictionary<string, Quest> old_glossary = null
        ){
            data = old_glossary == null ? new Dictionary<string, Quest>() : old_glossary;

            string quest_identifier = "collect_five_arrows";
            data.Add(
                quest_identifier,
                new Quest(
                    quest_identifier,//identifier
                    Event_moveItemToInventory,//eventToListenFor
                    GameHandler,//GameHandler
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
