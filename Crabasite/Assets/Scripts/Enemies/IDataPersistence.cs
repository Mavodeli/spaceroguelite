using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);
    // ref GameData because we want to change the GameData whereas in Load
    // we only want to read it.
    void SaveData(ref GameData data);
}
