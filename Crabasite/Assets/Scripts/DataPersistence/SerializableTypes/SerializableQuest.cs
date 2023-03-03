using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SerializableQuest : Quest, ISerializationCallbackReceiver  
{
    [SerializeField] private UnityEvent serialized_onCompletion = new UnityEvent();
    [SerializeField] private UnityEvent serialized_modifyCompletion = new UnityEvent();

    public SerializableQuest(Quest quest){
        identifier = quest.identifier;
        completed = quest.completed;
        modifyCompletion = quest.modifyCompletion;
        onCompletion = quest.onCompletion;
        GameHandler = quest.GameHandler;
        eventToListenFor = quest.eventToListenFor;
    }

    public void OnBeforeSerialize()
    {
        serialized_onCompletion.AddListener(() => this.onCompletion());
        serialized_modifyCompletion.AddListener(() => this.modifyCompletion());
    }

    public void OnAfterDeserialize()
    {
        this.onCompletion = delegate(){serialized_onCompletion.Invoke();};
        this.modifyCompletion = delegate(){serialized_modifyCompletion.Invoke();};
        serialized_onCompletion.RemoveListener(() => this.onCompletion());
        serialized_modifyCompletion.RemoveListener(() => this.modifyCompletion());
    }
}
