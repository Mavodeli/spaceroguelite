using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest
    {
        public delegate bool CompletionCriterion();
        public delegate void ModifyCompletion();
        public delegate void OnCompletion();
        public delegate void ReferencedEventAddListener();
        public delegate void ReferencedEventRemoveListener();
        public delegate void ReferencedEventDestroy();

        public string identifier;
        public bool completed;
        public OnCompletion onCompletion;
        public ModifyCompletion modifyCompletion;
        public ReferencedEventAddListener addListener;
        public ReferencedEventRemoveListener removeListener;
        public ReferencedEventDestroy destroyEvent;
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
                eventToListenFor?.AddListener(checkCompletion);
                // Debug.Log("added a new Listener, yay!");
                Debug.Log(eventToListenFor);
            };
            removeListener = delegate(){
                eventToListenFor?.RemoveListener(checkCompletion);
                // Debug.Log("Listener should be removed now...");
            };
            destroyEvent = delegate(){
                eventToListenFor = null;
                try
                {
                    eventToListenFor.AddListener(() => {});
                }
                catch (System.NullReferenceException)
                {
                    Debug.Log("eventToListenFor destroyed successfully");
                }
            };
        }

        // public Quest(){
        //     // Debug.Log("empty Quest constructor");
        // }

        public void activate(){
            removeListener();
            addListener();

            //if evt != null, aka on QuestJournal.LoadData
            // evt?.RemoveListener(checkCompletion);
            // evt?.AddListener(checkCompletion);
            Debug.Log("Quest "+identifier+" activated");
        }

        protected void checkCompletion(){
            bool previous_completed = completed;
            modifyCompletion();
            if(!previous_completed && completed){//do if just completed
                onCompletion();
                // removeListener();
                destroyEvent();
                GameHandler.SendMessage("updateStatusForCompletedQuest", identifier, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
