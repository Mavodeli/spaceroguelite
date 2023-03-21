using System.Collections.Generic;

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
}
