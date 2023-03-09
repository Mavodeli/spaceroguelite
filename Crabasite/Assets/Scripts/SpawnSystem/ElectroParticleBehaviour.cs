using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroParticleBehaviour : PhysicalEntity
{
    private AudioSource pickupSound;
    private InteractionButton ib;
    private OnPickup onPickup;
    private Item scriptable;

    public void Start(){

        //setup gameObject
        gameObject.name = name;
        gameObject.tag = "Collectable";

        //setup sound
        pickupSound = (AudioSource) (GameObject.Find("PickupObject")).GetComponent(typeof (AudioSource));

        //setup Pickup (formerly done by the PickupHandler)
        ib = gameObject.AddComponent<InteractionButton>();
        ib.Setup(delegate () {
            pickupSound.Play();
            
            //optional
            // GameObject player = GameObject.FindGameObjectWithTag("Player");
            // player.SendMessage("addHealth", -10, SendMessageOptions.DontRequireReceiver);

            if(!GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                InventoryManager.Instance.AddItem("ElectroParticle");
                Destroy(gameObject);
            }
        }, "e");
        ib.setNewOffset(new Vector3(0, 0, 0));
    }
}
