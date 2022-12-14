using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEnabler : MonoBehaviour
{
    private float distanceToPlayer = Mathf.Infinity;
    private float pickupDistanceMaximum = 3;
    private GameObject player;
    private GameObject button;
    private bool hasButton = false;
    private GameObject inventory;

    // void Awake(){
    //     inventory = GameObject.FindGameObjectWithTag("Inventory");
    // }

    void Start()
    {
        //get player via tag
        player = GameObject.FindWithTag("Player");
        button = Resources.Load<GameObject>("Prefabs/Inventory/button");
        //create button template
        // button = new GameObject("Pickup Button of " + name);
        // SpriteRenderer renderer = button.AddComponent<SpriteRenderer>();
        // renderer.sprite = Resources.Load<Sprite>("Sprites/ShortKey");
        // button.SetActive(false);
    }

    void Update(){
        if(player.GetComponent<PlayerHealth>().isAlive()){
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            //create button if not present and player is close enough
            //TODO: change button sprite
            if((distanceToPlayer <= pickupDistanceMaximum) && !hasButton){
                Vector3 offset = new Vector3(1, 1, 0);
                // button.SetActive(true);
                button.name = "Pickup Button of " + name;
                button.GetComponent<SpriteRenderer>().sortingOrder = 1;
                Instantiate(button, transform.position+offset, Quaternion.identity, transform);
                hasButton = true;
                // button.SetActive(false);
            }
            //destroy button if player is too far away
            if((distanceToPlayer > pickupDistanceMaximum) && hasButton){
                Destroy(transform.Find("Pickup Button of " + name + "(Clone)").gameObject);
                hasButton = false;
            }
            //check if player presses button for pickup, if so, perform pickup 

            if(Input.GetKey("e") && hasButton && !GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().inventoryIsOpened){
                if(this.transform.gameObject.GetComponent<ItemPickup>() == null){
                    ItemPickup ip = this.transform.gameObject.AddComponent<ItemPickup>();
                    Item placeholder = ScriptableObject.CreateInstance<Item>();
                    placeholder.id = 0;//time?
                    placeholder.itemName = name;
                    placeholder.description = "";//json dict?
                    placeholder.icon = this.transform.gameObject.GetComponent<SpriteRenderer>().sprite;
                    ip.target = placeholder;
                    ip.Pickup();
                }
            }
        }
    }
}