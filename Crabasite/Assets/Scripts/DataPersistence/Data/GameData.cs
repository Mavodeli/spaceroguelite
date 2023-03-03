using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameData 
{
    public int health;
    public int mana;
    public string level;
    public int lastEquippedUltimate;
    public SerializableDictionary<string, int> ItemsDict;
    public SerializableDictionary<string, bool> MailDict;
    public SerializableDictionary<string, bool> ProgressionDict;
    public SerializableDictionary<int, bool> UltimateDict;

    // QuestSystem
    public SerializableDictionary<string, SerializableQuest> activeQuests;
    public SerializableDictionary<string, SerializableQuest> completedQuests;
    public SerializableEvent Event_moveItemToInventory;
    

    // the values defined in this constructor will be the default values
    // the game starts with when there is no data to Load
    public GameData()
    {
        this.health = 100;
        this.mana = 100;
        this.level = "Level 1 - space";
        this.lastEquippedUltimate = 3;//'empty' ultimate
        this.ItemsDict = new SerializableDictionary<string, int>();
        this.MailDict = new SerializableDictionary<string, bool>();
        this.ProgressionDict = new SerializableDictionary<string, bool>();
        ProgressionDict.Add("triggeredEnemySpawner", false);
        this.UltimateDict = new SerializableDictionary<int, bool>();
        UltimateDict.Add(0, false);
        UltimateDict.Add(1, false);
        UltimateDict.Add(2, false);

        // QuestSystem
        this.activeQuests = new SerializableDictionary<string, SerializableQuest>();
        this.completedQuests = new SerializableDictionary<string, SerializableQuest>();
        this.Event_moveItemToInventory = new SerializableEvent();
    }
}
