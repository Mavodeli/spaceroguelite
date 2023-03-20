using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Quest
    {
        public delegate bool CompletionCriterion();
        public delegate void ModifyCompletion();
        public delegate void OnCompletion();

        private string identifier;
        private bool completed;
        private string eventToListenFor;
        private OnCompletion onCompletion;
        private ModifyCompletion modifyCompletion;
        private QuestJournal QJ;
        private QuestEvents QE;

        public Quest(string _identifier, string _eventToListenFor, CompletionCriterion completionCriterion, OnCompletion _onCompletion){
            // Debug.Log("default Quest constructor");
            identifier = _identifier;
            completed = false;
            eventToListenFor = _eventToListenFor;
            modifyCompletion = delegate(){
                completed = completionCriterion();
            };
            onCompletion = _onCompletion;
            QJ = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<QuestJournal>();
            QE = GameObject.FindGameObjectWithTag("QuestEventsContainer").GetComponent<QuestEvents>();
        }

        public void activate(){
            // QE.removeListener(eventToListenFor, QuestListener());
            QE.addListener(eventToListenFor, QuestListener());
        }

        public void deactivate(){
            QE.removeListener(eventToListenFor, QuestListener());
            // QE.addListener(eventToListenFor, QuestListener());
        }
        
        private Action QuestListener(){
            return (Action)(() => checkCompletion());
        }

        private void checkCompletion(){
            bool previous_completed = completed;
            modifyCompletion();
            if(!previous_completed && completed){//do if just completed
                onCompletion();
                GameObject.FindGameObjectWithTag("Inventory").SendMessage("RemoveQuestDescription", identifier, SendMessageOptions.DontRequireReceiver);
                QE.removeListener(eventToListenFor, QuestListener());
                QJ.updateStatusForCompletedQuest(identifier);
            }
        }
    }
