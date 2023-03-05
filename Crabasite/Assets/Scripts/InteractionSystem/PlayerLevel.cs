using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevel : MonoBehaviour, IDataPersistence
{
    public string level;

    public void LevelName()
    {
        level = SceneManager.GetActiveScene().name;
    }

    public void LoadData(GameData data)
    {
        LevelName();
        this.level = data.level;
    }
    public void SaveData(ref GameData data)
    {
        LevelName();
        data.level = this.level;
    }
}
