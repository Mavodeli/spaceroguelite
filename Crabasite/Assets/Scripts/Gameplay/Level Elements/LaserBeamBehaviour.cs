using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamBehaviour : MonoBehaviour
{
    SpriteRenderer sr;
    BoxCollider2D bc;
    
    void Start(){
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Vector2 emitter_position = new Vector2(transform.position.x, transform.position.y+sr.bounds.extents.y);
        Vector2 beam_direction = new Vector2(0, -1);

        RaycastHit2D hit = Physics2D.Raycast(emitter_position, beam_direction, sr.bounds.extents.y*2, LayerMask.GetMask("Raycast"));
        if(hit.collider != null){
            Debug.Log("hit "+hit.collider.gameObject.name);
            float beam_length = emitter_position.y-hit.point.y;
            float beam_end_position = emitter_position.y-beam_length;
            float new_beam_center = beam_end_position+((emitter_position.y-beam_end_position)/2);
            float max_beam_length = emitter_position.y-(sr.bounds.extents.y*2);
            bc.offset = new Vector2(0, new_beam_center-transform.position.y);
            // bc.size = new Vector2(bc.size.x, bc.size.y+(max_beam_length-beam_length));
        }
        else{
            bc.offset = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"){
            //some laser roasting player sound?
            other.gameObject.SendMessage("addHealth", -30, SendMessageOptions.DontRequireReceiver);
        }
    }
}
