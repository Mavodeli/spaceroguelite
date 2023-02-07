using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{

    private string name;
    public AudioClip clip;

    public Sound(string name, AudioClip clip){
        this.name = name;
        this.clip = clip;
    }

}
