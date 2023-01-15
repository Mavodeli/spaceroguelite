using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
   public int health;
   public List<string> Mails;

    // the values defined in this constructor will be the default values
    // the game starts with when there is no data to Load
   public GameData()
   {
       this.health = 100;
       this.Mails = new List<string>();
   }
}
