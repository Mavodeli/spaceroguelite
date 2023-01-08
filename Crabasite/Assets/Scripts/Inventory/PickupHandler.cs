using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    void Update()
    {
        //check if all collectables have the pickup script, if not, add missing scripts
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Collectable")){
            PickupEnabler script = item.GetComponent<PickupEnabler>();
            if(script == null){
                item.AddComponent<PickupEnabler>();
            }
        }
    }
}
