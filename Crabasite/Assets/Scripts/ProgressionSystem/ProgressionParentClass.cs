using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ProgressionParentClass : MonoBehaviour, IDataPersistence
{
    public delegate void OnTriggerEnterDelegate();
    protected ProgressionTracker PT = new ProgressionTracker();
    protected GameObject player;
    protected string enemySpawnPrefix = "EnemySpawn_";//DON'T CHANGE THIS VALUE, IT IS HARD-CODED IN THE DPM (LoadGame())!!!

    public void Awake(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void spawnItem(object[] args){

        if((string)args[0] == "CrushOrb"){
            Spawn.Item("CrushOrb", (Vector3)args[1], delegate(){
                CommentarySystem.displayProtagonistComment("onAnglerFishDied");
                int ult = 1;//crush
                GameObject IM = GameObject.FindGameObjectWithTag("Inventory");
                IM.SendMessage("unlockUltimate", ult, SendMessageOptions.DontRequireReceiver);
                player.SendMessage("SwitchUltimate", ult, SendMessageOptions.DontRequireReceiver);
            }, dontAddToInventory:true);
            return;
        }
        else if((string)args[0] == "NegativeChargeOrb"){
            Spawn.Item("NegativeChargeOrb", (Vector3)args[1], delegate(){
                int ult = 2;//neg. charge
                GameObject IM = GameObject.FindGameObjectWithTag("Inventory");
                IM.SendMessage("unlockUltimate", ult, SendMessageOptions.DontRequireReceiver);
                player.SendMessage("SwitchUltimate", ult, SendMessageOptions.DontRequireReceiver);
            }, dontAddToInventory:true);
            return;
        }
        else
            Spawn.Item((string)args[0], (Vector3)args[1]);
    }

    public void LoadData(GameData data)
    {
        PT.setFlagDict(data.ProgressionDict);
    }
    public void SaveData(ref GameData data)
    {
        data.ProgressionDict = (SerializableDictionary<string, bool>)PT.getFlagDict();
    }
}