using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionTracker : MonoBehaviour
{
    private Dictionary<string, bool> Flag = new Dictionary<string, bool>();

    /// <summary>
    /// Sets the value of the flag id to value.
    /// </summary>
    /// <param name="id">the name of the flag as string</param>
    /// <param name="value">the bool value that the flag with the given id should be set to</param>
    public void setFlag(string id, bool value){
        Flag[id] = value;
    }

    /// <summary>
    /// Returns true if the given flag is set to true, false if not or if it doesn't exist
    /// </summary>
    /// <param name="flag">the name of the flag as string</param>
    /// <returns>true if the given flag is set to true</returns>
    public bool isTrue(string flag){
        bool b = false;
        try{
            b = Flag[flag];
        }
        catch (KeyNotFoundException){
            b = false;
        }
        return b;
    }
}
