using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Negative Charge", menuName = "Ultimate/Create Negative Charge")]
public class NegativeCharge : Ultimate
{
    public GameObject selectedTarget;
    public KeyCode key;

    public override void Use()
    {
        selectedTarget = RayCastSelect.SelectTarget(key);

        if (selectedTarget is GameObject)
        {
            RepelCollider repelColliderScript = selectedTarget.AddComponent<RepelCollider>();
            selectedTarget = null;
            isActive = false;
        }
    }
}
