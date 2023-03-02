using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestJournal : MonoBehaviour, IDataPersistence
{
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    private Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();
    //serializable quests???

    //Listener Events
    public UnityEvent Event_moveItemToInventory = new UnityEvent();

    void Update(){
        // the next two lines I pulled straight from the deepest chasm of the performance hell
        // this way everything works fine but we might run into trouble with it later, depending 
        // on how many active quests we will end up with at the same time ;)
        // Dictionary<string, Quest> activeQuests_copy = new Dictionary<string, Quest>(activeQuests);
        // foreach(KeyValuePair<string, Quest> entry in activeQuests_copy){
        //     if(entry.Value.checkCompletion()){
        //         completedQuests.Add(entry.Key, entry.Value);
        //         activeQuests.Remove(entry.Key);
        //     }
        // }
    }

    private void InvokeEvent_moveItemToInventory(){
        Event_moveItemToInventory.Invoke();
    }

    private void updateStatusForCompletedQuest(string quest_identifier){
        try
        {
            completedQuests.Add(quest_identifier, activeQuests[quest_identifier]);
            activeQuests.Remove(quest_identifier);
        }
        catch (KeyNotFoundException)
        {
            Debug.Log("The quest "+quest_identifier+" is not an active Quest!");
        }
    }

    public void addNewQuest(Quest quest){
        try
        {
            activeQuests.Add(quest.identifier, quest);
        }
        catch (System.ArgumentException)
        {
            Debug.Log("Tried to add the Quest "+quest.identifier+" which already existed in activeQuests.");
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
        activeQuests.Clear();
        foreach(KeyValuePair<string, SerializableQuest> entry in data.activeQuests){
            activeQuests.Add(entry.Key, entry.Value);
        }
        completedQuests.Clear();
        foreach(KeyValuePair<string, SerializableQuest> entry in data.completedQuests){
            completedQuests.Add(entry.Key, entry.Value);
        }
    }
    
    public void SaveData(ref GameData data)
    {
        // data.activeQuests = (SerializableDictionary<string, Quest>)activeQuests;        
        // data.completedQuests = (SerializableDictionary<string, Quest>)completedQuests;
        data.activeQuests = new SerializableDictionary<string, SerializableQuest>();
        foreach(KeyValuePair<string, Quest> entry in activeQuests){
            SerializableQuest sq = new SerializableQuest(
                entry.Value.identifier,
                entry.Value.eventToListenFor,
                entry.Value.GameHandler,
                delegate(){entry.Value.onCompletion();}
            );
            data.activeQuests.Add(entry.Key, sq);
        }
        data.completedQuests = new SerializableDictionary<string, SerializableQuest>();
        foreach(KeyValuePair<string, Quest> entry in completedQuests){
            SerializableQuest sq = new SerializableQuest(
                entry.Value.identifier,
                entry.Value.eventToListenFor,
                entry.Value.GameHandler,
                delegate(){entry.Value.onCompletion();}
            );
            data.completedQuests.Add(entry.Key, sq);
        }
    }
}
