using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundParameter
{
        
    public string soundName;
    public GameObject gameObject;
    public float volume;
    public bool dontDestroyOnLoad;

    public SoundParameter(string soundName, GameObject gameObject, float volume, bool dontDestroyOnLoad){
        this.soundName = soundName;
        this.gameObject = gameObject;
        this.dontDestroyOnLoad = dontDestroyOnLoad;

        this.volume = volume;
        if(this.volume > 1f){
            this.volume = 1f;
        } else if(this.volume < 0f){
            this.volume = 0;
        }

    }
        
}

