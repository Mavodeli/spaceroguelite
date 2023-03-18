using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    // Gameplay related Savedata
    public int health;
    public int mana;
    public string level;
    public SerializableDictionary<string, int> ItemsDict;
    public SerializableDictionary<string, bool> MailDict;
    public SerializableDictionary<string, bool> QuestDict;
    public SerializableDictionary<string, bool> ProgressionDict;
    public SerializableDictionary<int, bool> UltimateDict;
    public int lastEquippedUltimate;
    public SerializableDictionary<string, bool> activeQuests;

    // Options related Savedata
    public int graphicsIndex;
    public float soundVolume;
    public bool isFullscreen;
    public int resolutionsIndex;

    // the values defined in this constructor will be the default values
    // the game starts with when there is no data to Load
    public GameData()
    {
        this.health = 100;
        this.mana = 100;
        this.level = "Level 1 - space";
        this.ItemsDict = new SerializableDictionary<string, int>();
        this.MailDict = new SerializableDictionary<string, bool>();
        this.QuestDict = new SerializableDictionary<string, bool>();
        this.ProgressionDict = new SerializableDictionary<string, bool>();
        ProgressionDict.Add("triggeredEnemySpawner", false);
        this.UltimateDict = new SerializableDictionary<int, bool>();
        UltimateDict.Add(0, false);
        UltimateDict.Add(1, false);
        UltimateDict.Add(2, false);
        this.lastEquippedUltimate = 3;
        this.activeQuests = new SerializableDictionary<string, bool>();
        this.graphicsIndex = 0;
        this.soundVolume = 0;
        this.isFullscreen = true;
        this.resolutionsIndex = 0;
    }
}
