using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{

    void Start(){

    }

    void Update()
    {
        //check if all collectables have the pickup script, if not, add missing scripts
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Collectable")){
            InteractionButton script = item.GetComponent<InteractionButton>();
            if(script == null){
                script = item.AddComponent<InteractionButton>();
                script.Setup(delegate () {
                    if(!GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                        InventoryManager.Instance.AddItem(item.name);
                        Destroy(item);
                    }
                }, "e");
            }
        }
    }
}
