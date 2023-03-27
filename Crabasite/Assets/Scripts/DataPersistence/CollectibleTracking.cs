using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleTracking : MonoBehaviour, IDataPersistence
{
    public Dictionary<string,bool> CollectibleDict = new Dictionary<string,bool>();

    public void deleteCollectibles()
    {
        if (SceneManager.GetActiveScene().name == "Level 1 - space")
        {
            foreach (var entry in CollectibleDict)
            {               
                Destroy(GameObject.Find(entry.Key));
            }
        }                 
    }

    public void AddCollectibleToDict(string collectible)
    {
        if (!CollectibleDict.ContainsKey(collectible))
        {
            CollectibleDict.Add(collectible, true);
        }
    }






    public void LoadData(GameData data)
    {
        CollectibleDict = data.CollectibleDict;
    }

    public void SaveData(ref GameData data)
    {
        data.CollectibleDict = (SerializableDictionary<string, bool>)CollectibleDict;
    }
}
