using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundParameter
{
        
    public string soundName;
    public GameObject gameObject;
    public bool dontDestroyOnLoad;

    public SoundParameter(string soundName, GameObject gameObject, bool dontDestroyOnLoad){
        this.soundName = soundName;
        this.gameObject = gameObject;
        this.dontDestroyOnLoad = dontDestroyOnLoad;
    }
        
}

