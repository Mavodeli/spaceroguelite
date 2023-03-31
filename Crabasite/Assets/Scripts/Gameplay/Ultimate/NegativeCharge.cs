using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Negative Charge", menuName = "Ultimate/Create Negative Charge")]
public class NegativeCharge : Ultimate
{
    public GameObject selectedTarget;
    public KeyCode key;

    public override void Use()
    {
        //select target if none is selected
        if (selectedTarget == null)
        {
            selectedTarget = RayCastSelect.SelectTarget(key);
        }
        else 
        {
            RepelCollider repelColliderScript = selectedTarget.AddComponent<RepelCollider>();
            selectedTarget = null;
            isActive = false;           
        }
        
    }

}
