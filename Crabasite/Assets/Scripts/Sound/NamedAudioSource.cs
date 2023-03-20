using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedAudioSource
{

    public string name;
    public AudioSource source;

    public NamedAudioSource(string name, AudioSource source){
        this.name = name;
        this.source = source;
    }
}
