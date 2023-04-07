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
    public SerializableDictionary<string, bool> CollectibleDict;
    public int lastEquippedUltimate;
    public SerializableDictionary<string, bool> activeQuests;
    public bool AS_EmergencyDoorUnlocked;
    public bool AS_hasGravity;
    public bool AS_GlassWallIntact;
    public bool AS_EngineRoomHatchLoose;
    public Vector3 AS_EngineRoomPosition;
    public Vector3 AS_EngineRoomRotation;
    public Vector3 AS_EngineRoomScale;


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
        this.UltimateDict = new SerializableDictionary<int, bool>();
        this.CollectibleDict = new SerializableDictionary<string, bool>();
        UltimateDict.Add(0, false);
        UltimateDict.Add(1, false);
        UltimateDict.Add(2, false);
        this.lastEquippedUltimate = 3;
        this.activeQuests = new SerializableDictionary<string, bool>();
        this.AS_EmergencyDoorUnlocked = false;
        this.AS_hasGravity = true;
        this.AS_GlassWallIntact = true;
        this.AS_EngineRoomHatchLoose = false;
        this.AS_EngineRoomPosition = new Vector3(-8, 27.9f, 0);
        this.AS_EngineRoomRotation = new Vector3(0, 0, 0);
        this.AS_EngineRoomScale = new Vector3(1.9f,0.2f,1);
    }
}
