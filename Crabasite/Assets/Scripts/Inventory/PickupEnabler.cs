using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEnabler : MonoBehaviour
{
    private float distanceToPlayer = Mathf.Infinity;
    private float pickupDistanceMaximum = 3;
    private GameObject player;
    public GameObject button;
    private bool hasButton = false;

    void Start()
    {
        //get player via tag
        player = GameObject.FindWithTag("Player");
        //create button template
        // button = new GameObject("Pickup Button of " + name);
        // SpriteRenderer renderer = button.AddComponent<SpriteRenderer>();
        // renderer.sprite = Resources.Load<Sprite>("Sprites/ShortKey");
        // button.SetActive(false);
    }

    void Update(){
        if(player){
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            //create button if not present and player is close enough
            //TODO: change button sprite
            if((distanceToPlayer <= pickupDistanceMaximum) && !hasButton){
                Vector3 offset = new Vector3(1, 1, 0);
                // button.SetActive(true);
                button.name = "Pickup Button of " + name;
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
            if(Input.GetKey("e") && hasButton){
                if(this.transform.gameObject.GetComponent<ItemPickup>() == null){
                    ItemPickup ip = this.transform.gameObject.AddComponent<ItemPickup>();
                    Item placeholder = Resources.Load<Item>("ScriptableObjects/demoItem");
                    placeholder.id = 0;
                    placeholder.itemName = name;
                    placeholder.description = "";
                    placeholder.icon = this.transform.gameObject.GetComponent<SpriteRenderer>().sprite;
                    ip.target = placeholder;
                    ip.Pickup();
                }
            }
        }
    }
}