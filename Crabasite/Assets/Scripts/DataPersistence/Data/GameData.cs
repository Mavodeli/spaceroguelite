using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int health;
    public SerializableDictionary<string, int> ItemsDict;
    public SerializableDictionary<string, bool> MailDict;
    public SerializableDictionary<string, bool> ProgressionDict;

    // the values defined in this constructor will be the default values
    // the game starts with when there is no data to Load
    public GameData()
    {
        this.health = 100;
        this.ItemsDict = new SerializableDictionary<string, int>();
        this.MailDict= new SerializableDictionary<string, bool>();
        this.ProgressionDict = new SerializableDictionary<string, bool>();
    }
}