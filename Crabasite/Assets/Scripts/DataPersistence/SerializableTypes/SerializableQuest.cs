using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SerializableQuest : Quest, ISerializationCallbackReceiver  
{
    [SerializeField] private string serialized_identifier;
    [SerializeField] private bool serialized_completed;
    [SerializeField] private UnityEvent serialized_onCompletion;
    [SerializeField] private UnityEvent serialized_modifyCompletion;
    [SerializeField] private UnityEvent serialized_eventToListenFor;
    [SerializeField] private GameObject serialized_GameHandler;

    public SerializableQuest(string _identifier, UnityEvent _eventToListenFor, GameObject _GameHandler, OnCompletion _onCompletion) : base(_identifier, _eventToListenFor, _GameHandler, delegate(){return false;}, _onCompletion){

    }

    public void OnBeforeSerialize()
    {
        serialized_identifier = this.identifier;
        serialized_completed = this.completed;
        serialized_onCompletion.AddListener(() => this.onCompletion());
        serialized_modifyCompletion.AddListener(() => this.modifyCompletion());
        serialized_eventToListenFor = this.eventToListenFor;
        serialized_GameHandler = this.GameHandler;
    }

    public void OnAfterDeserialize()
    {
        this.identifier = serialized_identifier;
        this.completed = serialized_completed;
        this.onCompletion = serialized_onCompletion.Invoke;
        this.modifyCompletion = serialized_modifyCompletion.Invoke;
        this.eventToListenFor = serialized_eventToListenFor;
        this.GameHandler = serialized_GameHandler; 
    }
}
