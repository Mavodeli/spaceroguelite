using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public class FlagDict
{
    public Dictionary<string, bool> Flag;

    public FlagDict(Dictionary<string, bool> dict)
    {
        Flag = dict;
    }
}

public class ProgressionTracker
{
    public static void initProgressionTracker(){
        SaveData(new Dictionary<string, bool>());
    }

    /// <summary>
    /// Sets the value of the flag 'id' to 'value'.
    /// </summary>
    /// <param name="id">the name of the flag as string</param>
    /// <param name="value">the bool value that the flag with the given id should be set to. default is true</param>
    public static void setFlag(string id, bool value = true){
        Dictionary<string, bool> Flag = LoadData();
        Flag[id] = value;
        SaveData(Flag);
    }

    /// <summary>
    /// Returns true if the given flag 'flag' is set to true in the ProgressionTracker, false if not or if it doesn't exist
    /// </summary>
    /// <param name="id">the name of the flag as string</param>
    /// <returns>true if the given flag is set to true</returns>
    public static bool getFlag(string id)
    {
        Dictionary<string, bool> Flag = LoadData();
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

    private static void SaveData(Dictionary<string, bool> dict)
    {
        //%userprofile%/AppData/LocalLow/DefaultCompany/Crabasite
        string destination = Application.persistentDataPath + "/FlagDictData.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        FlagDict data = new FlagDict(dict);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    private static Dictionary<string, bool> LoadData()
    {
        string destination = Application.persistentDataPath + "/FlagDictData.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return new Dictionary<string, bool>();
        }

        BinaryFormatter bf = new BinaryFormatter();
        FlagDict data = (FlagDict)bf.Deserialize(file);
        file.Close();
        return data.Flag;
    }
}
