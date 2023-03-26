using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroParticleBehaviour : PhysicalEntity
{
    private AudioSource pickupSound;
    private InteractionButton ib;

    public void Start(){

        //setup sound
        pickupSound = (AudioSource) (GameObject.Find("PickupObject")).GetComponent(typeof (AudioSource));

        GameObject GameHandler = GameObject.FindGameObjectWithTag("GameHandler");

        //setup Pickup (formerly done by the PickupHandler)
        ib = gameObject.AddComponent<InteractionButton>();
        ib.Setup(delegate () {
            //optional
            // GameObject player = GameObject.FindGameObjectWithTag("Player");
            // player.SendMessage("addHealth", -10, SendMessageOptions.DontRequireReceiver);

            if(!GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                pickupSound.Play();
                InventoryManager.Instance.AddItem("ElectroParticle");
                GameHandler.GetComponent<CollectibleTracking>().AddCollectibleToDict(name); 
                Debug.Log(GameHandler);
                Destroy(gameObject);
            }
        }, "e");
        ib.setNewOffset(new Vector3(0, 0, 0));
    }
}
