using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilicateBehaviour : PhysicalEntity
{
    private AudioSource pickupSound;
    private InteractionButton ib;

    public void Start(){

        //setup sound
        pickupSound = (AudioSource) (GameObject.Find("PickupObject")).GetComponent(typeof (AudioSource));

        //setup Pickup (formerly done by the PickupHandler)
        ib = gameObject.AddComponent<InteractionButton>();
        ib.Setup(delegate () {
            pickupSound.Play();

            if(!GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                InventoryManager.Instance.AddItem("Silicate");
                Destroy(gameObject);
            }
        }, "e");
        ib.setNewOffset(new Vector3(0, 0, 0));
    }
}
