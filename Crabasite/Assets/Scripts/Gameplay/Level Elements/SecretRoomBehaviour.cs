using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoomBehaviour : MonoBehaviour
{
    GameObject player;
    BoxCollider2D bc;
    GameObject PC;
    GameObject sign;
    GameObject bg;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        bc = gameObject.GetComponent<BoxCollider2D>();
        PC = transform.GetChild(0).gameObject;
        sign = transform.GetChild(1).gameObject;
        bg = transform.GetChild(2).gameObject;
        OnTriggerExit2D(player.GetComponent<Collider2D>());
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            PC.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            sign.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            bg.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            PC.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            sign.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            bg.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }
}
