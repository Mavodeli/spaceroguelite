using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestJournal : MonoBehaviour//, IDataPersistence
{
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    private Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();
    //serializable quests???

    void Update(){
        // the next two lines I pulled straight from the deepest chasm of the performance hell
        // this way everything works fine but we might run into trouble with it later, depending 
        // on how many active quests we will end up with at the same time ;)
        Dictionary<string, Quest> activeQuests_copy = new Dictionary<string, Quest>(activeQuests);
        foreach(KeyValuePair<string, Quest> entry in activeQuests_copy){
            if(entry.Value.checkCompletion()){
                completedQuests.Add(entry.Key, entry.Value);
                activeQuests.Remove(entry.Key);
            }
        }
    }

    public void addNewQuest(Quest quest){
        activeQuests.Add(quest.identifier, quest);
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

    public struct Quest
    {
        public string identifier;
        public delegate bool CompletionCriterion();
        public delegate void OnCompletion();

        private bool completed;
        private OnCompletion onCompletion;
        private CompletionCriterion completionCriterion;

        public Quest(string name, CompletionCriterion _completionCriterion = null, OnCompletion _onCompletion = null){
            identifier = name;
            completed = false;
            completionCriterion = _completionCriterion != null ? _completionCriterion : delegate(){return true;};
            onCompletion = _onCompletion != null ? _onCompletion : delegate(){};
        }

        public bool checkCompletion(){
            bool meetsCompletionCriterion = completionCriterion();
            if(!completed && meetsCompletionCriterion) onCompletion();//do if just completed
            completed = completed || meetsCompletionCriterion;
            return completed;
        }
    }
}
