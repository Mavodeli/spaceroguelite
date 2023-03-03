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
        public delegate void ReferencedEventAddListener();
        public delegate void ReferencedEventRemoveListener();

        public bool completed;
        public OnCompletion onCompletion;
        public ModifyCompletion modifyCompletion;
        public ReferencedEventAddListener addListener;
        public ReferencedEventRemoveListener removeListener;
        public GameObject GameHandler;

        public Quest(string _identifier, UnityEvent eventToListenFor, GameObject _GameHandler, CompletionCriterion completionCriterion, OnCompletion _onCompletion){
            // Debug.Log("default Quest constructor");
            identifier = _identifier;
            completed = false;
            modifyCompletion = delegate(){
                completed = completionCriterion();
            };
            onCompletion = _onCompletion;
            GameHandler = _GameHandler;

            addListener = delegate(){
                eventToListenFor.AddListener(checkCompletion);
                // Debug.Log("added a new Listener, yay!");
                // Debug.Log(eventToListenFor);
            };
            removeListener = delegate(){
                eventToListenFor.RemoveListener(checkCompletion);
                // Debug.Log("Listener should be removed now...");
            };
        }

        public Quest(){
            // Debug.Log("empty Quest constructor");
        }

        public void activate(){
            removeListener();
            addListener();

            //if evt != null, aka on QuestJournal.LoadData
            // evt?.RemoveListener(checkCompletion);
            // evt?.AddListener(checkCompletion);
            // Debug.Log("Quest "+identifier+" activated");
        }

        protected void checkCompletion(){
            bool previous_completed = completed;
            modifyCompletion();
            if(!previous_completed && completed){//do if just completed
                Debug.Log("checkCompletion successful");
                onCompletion();
                removeListener();
                GameHandler.SendMessage("updateStatusForCompletedQuest", identifier, SendMessageOptions.DontRequireReceiver);
            }
            else{
                Debug.Log("checkCompletion failed");
            }
        }
    }
