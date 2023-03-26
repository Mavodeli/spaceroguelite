using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilicateBehaviour : PhysicalEntity
{
    private AudioSource pickupSound;
    private InteractionButton ib;

    public void Start(){

        //fix sprite scaling
        GetComponent<SpriteRenderer>().size *= 8;

        //setup sound
        pickupSound = (AudioSource) (GameObject.Find("PickupObject")).GetComponent(typeof (AudioSource));

        GameObject GameHandler = GameObject.FindGameObjectWithTag("GameHandler");

        //setup Pickup (formerly done by the PickupHandler)
        ib = gameObject.AddComponent<InteractionButton>();
        ib.Setup(delegate () {
            if(!GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                pickupSound.Play();
                InventoryManager.Instance.AddItem("Silicate");                
                GameHandler.GetComponent<CollectibleTracking>().AddCollectibleToDict(name);
                Destroy(gameObject);
            }
        }, "e");
        ib.setNewOffset(new Vector3(0, 0, 0));
    }
}
