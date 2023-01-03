using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisShrimpSpear : MonoBehaviour
{
    private MantisShrimpData msd;
    private GameObject parent;
    private Vector3 targetDirection;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public void Setup(GameObject _parent, GameObject _target, Sprite _sprite){
        msd = Resources.Load<MantisShrimpData>("Scriptable Objects/EnemyData/MantisShrimpData");

        parent = _parent;
        gameObject.transform.parent = parent.transform;
        gameObject.transform.position = parent.transform.position;
        targetDirection = (_target.transform.position-gameObject.transform.position).normalized;
        gameObject.transform.Rotate(new Vector3(0, 0, 90));//rotate such that the point is up
        float angle = Vector3.Angle(new Vector3(0, 1, 0), targetDirection);
        gameObject.transform.Rotate(new Vector3(0, 0, angle));
        gameObject.name = "Mantis Shrimp Spear";
        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Raycast");

        //setup SpriteRenderer
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        sr.drawMode = SpriteDrawMode.Sliced;//needed for scaling the sprite
        sr.size *= msd.textureScaleSpear;
        sr.sortingOrder = 1;

        //setup Rigidbody2D
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/EnemyMaterial");

        //setup BoxCollider2D
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = sr.size;
    }

    void Update(){
        rb.AddForce(targetDirection*msd.spearSpeed*Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.gameObject != parent){
            if((collision.collider.gameObject.tag == "Player") ||
                (collision.collider.gameObject.tag == "Enemy")
                ){
                collision.collider.SendMessage("addHealth", -msd.damage, SendMessageOptions.DontRequireReceiver);
            }
            GameObject.Destroy(gameObject);
        }
    }
}