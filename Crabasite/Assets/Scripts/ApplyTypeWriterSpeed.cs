using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Yes this script is not sorted into a folder. This way it can reference both 
// DataPersistenceManager and CommentarySystem without messing with the Assembly Referencing

public class ApplyTypeWriterSpeed : MonoBehaviour
{
    private DataPersistenceManager dpm;

    // Start is called before the first frame update
    void Start()
    {
        dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        GameData gd = dpm.getGameData(false);

        gameObject.GetComponent<CommentarySystem>().setTypeWriterSpeed(gd.textSpeed);
    }
}
