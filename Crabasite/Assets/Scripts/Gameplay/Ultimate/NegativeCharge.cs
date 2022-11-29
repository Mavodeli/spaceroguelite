using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NegativeCharge : Ultimate
{
    public GameObject selectedTarget;
    public KeyCode key;

    public override void Use()
    {
        //select target if none is selected
        // Debug.Log(selectedTarget);
        if (selectedTarget == null)
        {
            selectedTarget = RayCastSelect.SelectTarget(key);
        }
        else // attach a repelCollider, which infuses the object with a "negative charge"
        {            
            RepelCollider repelColliderScript = selectedTarget.AddComponent<RepelCollider>();
            selectedTarget = null;
            isActive = false;           
        }
        // Debug.Log(selectedTarget);
    }

}
