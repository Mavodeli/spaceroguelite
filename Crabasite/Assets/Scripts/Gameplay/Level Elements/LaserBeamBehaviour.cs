using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamBehaviour : MonoBehaviour
{
    SpriteRenderer sr;
    BoxCollider2D bc;
    float max_beam_length;
    Vector2 emitter_position;
    
    void Start(){
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        sr.size *= new Vector2(sr.transform.localScale.x, sr.transform.localScale.y);
        bc = GetComponent<BoxCollider2D>();
        bc.size = sr.size;
        max_beam_length = sr.size.y;
        emitter_position = new Vector2(transform.position.x, transform.position.y+sr.size.y/2);
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(emitter_position, new Vector2(0, -1), max_beam_length, LayerMask.GetMask("Raycast"));
        if(hit.collider != null){
            float current_beam_length = emitter_position.y-hit.point.y;
            float beam_center_modifier = (emitter_position.y-(current_beam_length/2))-transform.position.y;
            Debug.DrawRay(emitter_position, hit.point-emitter_position);
            
            bc.offset = new Vector2(0, beam_center_modifier);
            bc.size = new Vector2(bc.size.x, current_beam_length);
            squashSprite(current_beam_length/max_beam_length, beam_center_modifier);
        }
        else{
            bc.offset = new Vector2(0, 0);
            bc.size = new Vector2(bc.size.x, max_beam_length);
            squashSprite(1, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"){
            //some laser roasting player sound?
            other.gameObject.SendMessage("addHealth", -90, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void squashSprite(float scale_modifier, float transform_modifier){
        GameObject child = transform.GetChild(0).gameObject;
        child.transform.localScale = new Vector3(1, scale_modifier, 0);
        child.transform.localPosition = new Vector3(0, transform_modifier, 0);
    }
}
