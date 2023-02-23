using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionParentClass : MonoBehaviour, IDataPersistence
{
    public delegate void OnTriggerEnterDelegate();
    protected ProgressionTracker PT = new ProgressionTracker();

    protected void send_mail(string id){
        InventoryManager IM = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
        IM.AddMail(id);
    }

    public void LoadData(GameData data)
    {
        PT.setFlagDict(data.ProgressionDict);
    }
    public void SaveData(ref GameData data)
    {
        data.ProgressionDict = (SerializableDictionary<string, bool>) PT.getFlagDict();
    }
}