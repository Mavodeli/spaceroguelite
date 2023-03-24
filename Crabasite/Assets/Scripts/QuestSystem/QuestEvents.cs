using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class QuestEvents : MonoBehaviour
{
    private Dictionary<string, UnityEvent> Events;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);

        if(Events == null){
            Events = new Dictionary<string, UnityEvent>();
            Events.Add("moveItemToInventory", new UnityEvent());
            Events.Add("interactedWithWindshield", new UnityEvent());
            Events.Add("interactedWithWorkbench", new UnityEvent());
            Events.Add("interactedWithHyperdrive", new UnityEvent());
            Events.Add("unlockUltimate", new UnityEvent());
            Events.Add("interactedWithEmergencyDoor", new UnityEvent());
        }
    }

    private void InvokeEvent(string identifier){
        if(idIsValid(identifier))
            Events[identifier]?.Invoke();
    }

    public UnityEvent getEvent(string identifier){
        UnityEvent evt = new UnityEvent();
        if(idIsValid(identifier))
            evt = Events[identifier];
        return evt;
    }

    public void addListener(string identifier, Action action){
        if(idIsValid(identifier))
            Events[identifier]?.AddListener(() => action());
    }

    public void removeListener(string identifier, Action action){
        if(idIsValid(identifier))
            Events[identifier]?.RemoveAllListeners();
            // Events[identifier]?.RemoveListener(() => action());
    }

    private bool idIsValid(string identifier){
        bool b;
        try
        {
            UnityEvent evt = Events[identifier];
            b = true;
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError("QuestEvent "+identifier+" not found");
            b = false;
        }
        return b;
    }
}
