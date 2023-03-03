using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest
    {
        public string identifier;
        public delegate bool CompletionCriterion();
        public delegate void ModifyCompletion();
        public delegate void OnCompletion();

        public bool completed;
        public OnCompletion onCompletion;
        public ModifyCompletion modifyCompletion;
        public GameObject GameHandler;
        public UnityEvent eventToListenFor;

        public Quest(string _identifier, UnityEvent _eventToListenFor, GameObject _GameHandler, CompletionCriterion completionCriterion, OnCompletion _onCompletion){
            identifier = _identifier;
            completed = false;
            modifyCompletion = delegate(){
                completed = completionCriterion();
            };
            onCompletion = _onCompletion;
            GameHandler = _GameHandler;
            eventToListenFor = _eventToListenFor;
            eventToListenFor.AddListener(checkCompletion);
        }

        public Quest(){}

        protected void checkCompletion(){
            bool previous_completed = completed;
            modifyCompletion();
            if(!previous_completed && completed){//do if just completed
                onCompletion();
                eventToListenFor.RemoveListener(checkCompletion);
                GameHandler.SendMessage("updateStatusForCompletedQuest", identifier, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
