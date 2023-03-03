using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestJournal : MonoBehaviour, IDataPersistence
{   
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    private Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();

    // Events (don't forget to modify GameData.cs)
    public SerializableEvent Event_moveItemToInventory = new SerializableEvent();

    private void InvokeEvent_moveItemToInventory(){
        Event_moveItemToInventory?.Invoke();
    }

    private void updateStatusForCompletedQuest(string quest_identifier){
        try
        {
            completedQuests.Add(quest_identifier, activeQuests[quest_identifier]);
            activeQuests.Remove(quest_identifier);
        }
        catch (KeyNotFoundException)
        {
            // Debug.LogWarning("The quest "+quest_identifier+" is not an active Quest!");
        }
    }

    public void addNewQuest(Quest quest){

        if(QuestIsCompleted(quest.identifier)){
            // Debug.LogWarning("Tried to add the Quest "+quest.identifier+" which has already been completed. The new Quest was not added.");
            return;
        }

        try
        {
            activeQuests.Add(quest.identifier, quest);
            quest.activate();//took 3 days to add this line ;) (hopefully everything works fine now...)
        }
        catch (System.ArgumentException)
        {
            // Debug.LogWarning("Tried to add the Quest "+quest.identifier+" which already existed in activeQuests. The new Quest was not added.");
        }
    }

    public bool QuestIsCompleted(string quest_identifier){
        bool b;
        try
        {
            Quest tmp = completedQuests[quest_identifier];
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
        // Events (don't forget to modify GameData.cs)
        Event_moveItemToInventory = data.Event_moveItemToInventory;

        activeQuests.Clear();
        foreach(KeyValuePair<string, SerializableQuest> entry in data.activeQuests){
            activeQuests.Add(entry.Key, entry.Value);
            entry.Value.activate();//!!!
        }
        completedQuests.Clear();
        foreach(KeyValuePair<string, SerializableQuest> entry in data.completedQuests){
            completedQuests.Add(entry.Key, entry.Value);
        }
    }
    
    public void SaveData(ref GameData data)
    {
        data.activeQuests = new SerializableDictionary<string, SerializableQuest>();
        foreach(KeyValuePair<string, Quest> entry in activeQuests){
            data.activeQuests.Add(entry.Key, new SerializableQuest(entry.Value));
        }
        data.completedQuests = new SerializableDictionary<string, SerializableQuest>();
        foreach(KeyValuePair<string, Quest> entry in completedQuests){
            data.completedQuests.Add(entry.Key, new SerializableQuest(entry.Value));
        }

        // Events (don't forget to modify GameData.cs)
        data.Event_moveItemToInventory = Event_moveItemToInventory;
    }
}
