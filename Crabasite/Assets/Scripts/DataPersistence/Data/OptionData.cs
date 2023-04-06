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

    // default values
    public OptionData()
    {
        this.graphicsIndex = 0;
        this.resolutionsIndex = 0;
        this.isFullscreen = false;
        this.soundVolume = 0.8f; // Range 0 - 1
        this.textSpeed = 200.0f; // characters per second
    }
}
