using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundParameter
{
    
    public string soundName;
    public GameObject gameObject;
    public bool destroyOnLoad;

    public SoundParameter(string soundName, GameObject gameObject, bool destroyOnLoad){
        this.soundName = soundName;
        this.gameObject = gameObject;
        this.destroyOnLoad = destroyOnLoad;
    }
    
}
