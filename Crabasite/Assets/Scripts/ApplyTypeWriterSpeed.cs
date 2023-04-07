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
        OptionData optionData = OptionPersistenceManager.instance.GetOptionData();
        gameObject.GetComponent<CommentarySystem>().setTypeWriterSpeed(optionData.textSpeed);
    }
}
