using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : PhysicalEntity
{
    private AudioSource pickupSound;
    private InteractionButton ib;
    private OnPickup onPickup;
    private Item scriptable;

    public void Setup(string name, OnPickup _onPickup = null, bool dontAddToInventory = false){

        //check ScriptableObject existence
        try
        {
            scriptable = Resources.Load<Item>("ScriptableObjects/Items/"+name);
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("ERROR: The corresponding ScriptableObject for the Item "+name+" was not found. Aborting ItemBehaviour Setup.");
            return;
        }

        //setup gameObject
        gameObject.name = name;
        gameObject.tag = "Collectable";

        //setup SpriteRenderer
        sr.sprite = scriptable.icon;
        sr.size *= scriptable.iconScale;

        //setup delegate
        onPickup = delegate(){};
        if(_onPickup != null)
            onPickup = _onPickup;

        //setup sound
        pickupSound = (AudioSource) (GameObject.Find("PickupObject")).GetComponent(typeof (AudioSource));

        //setup Pickup (formerly done by the PickupHandler)
        ib = gameObject.AddComponent<InteractionButton>();
        ib.Setup(delegate () {
            if(!GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                pickupSound.Play();
                if(!dontAddToInventory) InventoryManager.Instance.AddItem(gameObject.name);
                onPickup();
                Destroy(gameObject);
            }
        }, "e");
        ib.setNewOffset(new Vector3(0, 0, 0));
    }
}
