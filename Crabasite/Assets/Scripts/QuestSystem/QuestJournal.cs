using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestJournal : MonoBehaviour//, IDataPersistence
{
    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    //serializable quests???

    void Update(){
        foreach(Quest quest in activeQuests){
            if(quest.checkCompletion()){
                activeQuests.Remove(quest);
                completedQuests.Add(quest);
            }
        }
    }

    public void addNewQuest(Quest quest){
        activeQuests.Add(quest);
    }

    public struct Quest
    {
        public string identifier;
        private bool completed;
        public delegate bool completionCriterion();
        public delegate void OnCompletion();
        private OnCompletion onCompletion;

        private List<completionCriterion> completionCriteria;

        public Quest(string name, List<completionCriterion> _completionCriteria = null, OnCompletion _onCompletion = null){
            identifier = name;
            completed = false;
            completionCriteria = _completionCriteria != null ? _completionCriteria : new List<completionCriterion>();
            onCompletion = _onCompletion != null ? _onCompletion : delegate(){};
        }

        public bool checkCompletion(){
            bool meets = meetsCompletionCriteria();//compute once bc O(n)
            if(!completed && meets) onCompletion();//do if just completed
            completed = completed || meets;
            return completed;
        }

        private bool meetsCompletionCriteria(){
            bool b = true;
            foreach(completionCriterion cc in completionCriteria){
                b = b && cc();
            }
            return b;
        }
    }
}
