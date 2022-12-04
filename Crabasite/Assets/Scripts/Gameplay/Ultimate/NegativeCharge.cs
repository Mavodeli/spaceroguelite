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
        if (selectedTarget == null)
        {
            selectedTarget = RayCastSelect.SelectTarget(key);
        }
        else 
        {            
            
            selectedTarget = null;
            isActive = false;           
        }
        
    }

}
