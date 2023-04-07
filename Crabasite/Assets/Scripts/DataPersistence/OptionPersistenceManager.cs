using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class OptionPersistenceManager : MonoBehaviour
{
    public static OptionPersistenceManager instance { get; set; }

    [Header("File Storage Config")]
    [SerializeField] private string fileName = "options.json";
    private OptionData optionData;

    private void Awake()
    { // We leave duplicate handling to the DataPersistenceManager
        if (!instance) { instance = this; }
    }

    public OptionData GetOptionData()
    {
        if (optionData == null)
        {
            optionData = LoadFromFile(); // load from file (default if no file)
            return optionData;
        }
        else // regular runtime load
        {
            return optionData;
        }
    }

    public void SetOptionData(OptionData optionData)
    {
        this.optionData = optionData;
        WriteToFile();
    }



    // file handling
    private OptionData LoadFromFile()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);

        OptionData optionData = null;
        if (!File.Exists(fullPath))
        {
            LoadDefaults(); // default options if no file exists
            return this.optionData; // return now loaded defaults
        }
        else
        {
            try
            {
                string readData = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        readData = reader.ReadToEnd();
                    }
                }
                optionData = JsonUtility.FromJson<OptionData>(readData);
            }
            catch (Exception e)
            {
                Debug.LogError("Error trying to load " + fullPath + " :\n" + e);
            }
        }
        if (optionData == null)
        {
            Debug.LogError("Loading options failed! using default values.");
            LoadDefaults();
            return this.optionData;
        }
        else
        {
            return optionData;
        }
    }

    private void WriteToFile()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string writeData = JsonUtility.ToJson(optionData, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(writeData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error trying to write options file " + fullPath + " :\n" + e);
        }
    }

    private void LoadDefaults()
    {
        optionData = new OptionData();
        WriteToFile();
    }
}
