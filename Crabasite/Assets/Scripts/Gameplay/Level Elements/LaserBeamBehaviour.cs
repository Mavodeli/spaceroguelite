using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamBehaviour : MonoBehaviour
{
    SpriteRenderer sr;
    BoxCollider2D bc;
    float max_beam_length;
    Vector2 emitter_position;
    Texture2D tex;
    
    void Start(){
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        tex = Resources.Load<Texture2D>("Sprites/MovableObjects/cargo_cont");//TODO set to correct path!!!
        sr.sprite = Sprite.Create(
            tex, //texture
            new Rect(0.0f, 0.0f, tex.width, tex.height), //subpart of the texture to create the sprite from
            new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
            100.0f, //number of pixels in the sprite that correspond to one unit in world space
            0, //amount by which the sprite mesh should be expanded outwards
            SpriteMeshType.FullRect //mesh type
        );
        max_beam_length = sr.bounds.extents.y*2;
        emitter_position = new Vector2(transform.position.x, transform.position.y+sr.bounds.extents.y);
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
            other.gameObject.SendMessage("addHealth", -30, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void squashSprite(float scale_modifier, float transform_modifier){
        GameObject child = transform.GetChild(0).gameObject;
        child.transform.localScale = new Vector3(1, scale_modifier, 0);
        child.transform.localPosition = new Vector3(0, transform_modifier, 0);
    }
}
