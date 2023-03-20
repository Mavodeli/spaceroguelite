using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipDebrisBehaviour : PhysicalEntity
{
    private AudioSource pickupSound;
    private InteractionButton ib;

    public void Start(){

        //setup sound
        pickupSound = (AudioSource) (GameObject.Find("PickupObject")).GetComponent(typeof (AudioSource));

        //setup Pickup (formerly done by the PickupHandler)
        ib = gameObject.AddComponent<InteractionButton>();
        ib.Setup(delegate () {
            if(!GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                pickupSound.Play();
                InventoryManager.Instance.AddItem("SpaceshipDebris");
                Destroy(gameObject);
            }
        }, "e");
        ib.setNewOffset(new Vector3(0, 0, 0));
    }
}
