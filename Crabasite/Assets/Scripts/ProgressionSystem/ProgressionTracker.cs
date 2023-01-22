using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using log4net.Filter;

/**
[System.Serializable]
public class DictionaryData
{
    public Dictionary<string, bool> Flag;

    public DictionaryData(Dictionary<string, bool> dict)
    {
        Flag = dict;
    }
}
**/

public class ProgressionTracker
{
    private Dictionary<string, bool> Flag = new Dictionary<string, bool>();
    
    public void setFlagDict(Dictionary<string, bool> newDict){
        Flag = newDict;
    }

    /// <summary>
    /// Sets the value of the flag 'id' to 'value'.
    /// </summary>
    /// <param name="id">the name of the flag as string</param>
    /// <param name="value">the bool value that the flag with the given id should be set to. default is true</param>
    public void setFlag(string id, bool value = true){
        Flag[id] = value;
    }

    /// <summary>
    /// Returns true if the given flag 'flag' is set to true in the ProgressionTracker, false if not or if it doesn't exist
    /// </summary>
    /// <param name="id">the name of the flag as string</param>
    /// <returns>true if the given flag is set to true</returns>
    public bool getFlag(string id)
    {
        bool b = false;
        try
        {
            b = Flag[id];
        }
        catch (KeyNotFoundException)
        {
            b = false;
        }
        return b;
    }

    public Dictionary<string, bool> getFlagDict(){
        return Flag;
    }

    /**
    private static void SaveData(Dictionary<string, bool> dict)
    {
        //%userprofile%/AppData/LocalLow/DefaultCompany/Crabasite
        string destination = Application.persistentDataPath + "/DictionaryData.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        DictionaryData data = new DictionaryData(dict);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    private static Dictionary<string, bool> LoadData()
    {
        string destination = Application.persistentDataPath + "/DictionaryData.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return new Dictionary<string, bool>();
        }

        BinaryFormatter bf = new BinaryFormatter();
        DictionaryData data = (DictionaryData)bf.Deserialize(file);
        file.Close();
        return data.Flag;
    }
    **/
}
