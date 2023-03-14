using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class CommentarySystem : MonoBehaviour
{
    public static void displayComment(string id){
        //TODO: implement this
        //@Rico: the string obtained with LoadFromFile(id) should appear in a textbox ingame and not as a debug log ;)
        Debug.Log(LoadFromFile(id));
    }

    private static string LoadFromFile(string identifier)
    {
        // using Path.Combine because of different Paths of different OS's
        string path = Path.Combine(Application.persistentDataPath, "english.json");
        string result = "";
        if(File.Exists(path))
        {
            Dictionary<string, string> deserializedData = new Dictionary<string, string>();
            try
            {
                string jsonData = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonData = reader.ReadToEnd();
                    }
                }
                deserializedData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
                result = deserializedData[identifier];
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file:" + path + "\n" + e);
            }
        }
        return result;
    }
}

