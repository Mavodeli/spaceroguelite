using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private GameObject parent;
    private Vector3 targetDirection;
    private float speed;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    public delegate void collision_delegate(Collider2D other);
    public collision_delegate del;

    public void Setup(GameObject _parent, 
                        GameObject _target, 
                        Sprite _sprite, 
                        float textureScale,
                        float _speed,
                        collision_delegate _delegate
                        ){
        parent = _parent;
        speed = _speed;
        del = _delegate;

        //position + direction
        gameObject.transform.parent = parent.transform;
        gameObject.transform.position = parent.transform.position;

        //determine facing direction
        targetDirection = (_target.transform.position-gameObject.transform.position).normalized;

        gameObject.name = parent.name+" Projectile";
        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Raycast");

        //setup SpriteRenderer
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        sr.drawMode = SpriteDrawMode.Sliced;//needed for scaling the sprite
        sr.size *= textureScale * new Vector2(.7f, 1);
        sr.sortingOrder = 1;

        //rotation
        gameObject.transform.right = _target.transform.position - transform.position;

        //setup Rigidbody2D
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/EnemyMaterial");

        //setup BoxCollider2D
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = sr.size;
        bc.isTrigger = true;

        //setup spawn position
        Vector3 Psr_extends = _parent.GetComponent<SpriteRenderer>().bounds.extents;
        float meanOffset = (Psr_extends.x+Psr_extends.y)/2;
        transform.position = _parent.transform.position + (targetDirection*meanOffset);
    }

    void FixedUpdate(){
        rb.AddForce(targetDirection*speed);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(
            (other.gameObject != parent) && 
            (other.gameObject.name != gameObject.name) &&
            (other.gameObject.tag != "ProgressionTrigger") &&
            (other.gameObject.tag != "AbandonedSpaceshipCollider")
        ){
            del(other);
            GameObject.Destroy(gameObject);
        }
    }
}