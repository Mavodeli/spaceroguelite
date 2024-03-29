using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData
{
    public int graphicsIndex;
    public int resolutionsIndex;
    public bool isFullscreen;
    public float soundVolume;
    public float textSpeed;
    public int vsync;

    // default values
    public OptionData()
    {
        this.graphicsIndex = 5; // Ultra - we are very performant :)
        this.resolutionsIndex = 0; // MainMenuManager handles this when defaulting
        this.isFullscreen = true; // matching unitys default
        this.soundVolume = 0.8f; // Range 0 - 1
        this.textSpeed = 200.0f; // characters per second
        this.vsync = 0; // v-sync is off by default
    }
}
