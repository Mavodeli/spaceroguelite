using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SerializableQuest : Quest, ISerializationCallbackReceiver  
{
    [SerializeField] private SerializableEvent serialized_onCompletion = new SerializableEvent();
    [SerializeField] private SerializableEvent serialized_modifyCompletion = new SerializableEvent();
    [SerializeField] private SerializableEvent serialized_addListener = new SerializableEvent();
    [SerializeField] private SerializableEvent serialized_removeListener = new SerializableEvent();

    public SerializableQuest(Quest quest){
        // Debug.Log("SerializableQuest copy constructor");
        identifier = quest.identifier;
        completed = quest.completed;
        modifyCompletion = quest.modifyCompletion;
        onCompletion = quest.onCompletion;
        GameHandler = quest.GameHandler;
        addListener = quest.addListener;
        removeListener = quest.removeListener;
    }

    public void OnBeforeSerialize()
    {
        serialized_onCompletion?.AddListener(() => this.onCompletion());
        serialized_modifyCompletion?.AddListener(() => this.modifyCompletion());
        serialized_addListener?.AddListener(() => this.addListener());
        serialized_removeListener?.AddListener(() => this.removeListener());
    }

    public void OnAfterDeserialize()
    {
        this.onCompletion = delegate(){serialized_onCompletion.Invoke();};
        this.modifyCompletion = delegate(){serialized_modifyCompletion.Invoke();};
        this.addListener = delegate(){serialized_addListener.Invoke();};
        this.removeListener = delegate(){serialized_removeListener.Invoke();};

        serialized_onCompletion?.RemoveListener(() => this.onCompletion());
        serialized_modifyCompletion?.RemoveListener(() => this.modifyCompletion());
        serialized_addListener?.RemoveListener(() => this.addListener());
        serialized_removeListener?.RemoveListener(() => this.removeListener());
    }
}
