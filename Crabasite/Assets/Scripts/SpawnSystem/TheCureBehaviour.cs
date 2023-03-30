using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCureBehaviour : PhysicalEntity
{
    private AudioSource pickupSound;
    private InteractionButton ib;
    private OnPickup onPickup;
    private Item scriptable;
    private HealthSystem HS;
    private Vector3 spawnPosition;

    public void Start(){

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

        HS = new HealthSystem(1, 1);
        spawnPosition = transform.position;

        //setup sound
        pickupSound = (AudioSource) (GameObject.Find("PickupObject")).GetComponent(typeof (AudioSource));

        //setup Pickup (formerly done by the PickupHandler)
        ib = gameObject.AddComponent<InteractionButton>();
        ib.Setup(delegate () {
            if(!GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                pickupSound.Play();
                InventoryManager.Instance.AddItem(gameObject.name);
                Destroy(gameObject);
            }
        }, "e");
        ib.setNewOffset(new Vector3(0, 0, 0));
    }

    public void Respawn(){
        //respawn if not picked up
        GameObject newCure = Instantiate(Resources.Load<GameObject>("Prefabs/Collectables/TheCure"));
        newCure.transform.position = spawnPosition;
    }

    private void addHealth(int hp){
        float health = Mathf.Clamp(HS.Health+hp,0,HS.MaxHealth);
        if(hp < 0) HS.Damage(-hp);
        else HS.Heal(hp);
        if(health == 0){
            Respawn();
            DataPersistenceManager.instance.LoadGame(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().GameOverScreen.Setup();
            Destroy(gameObject);
        } 
    }
}
