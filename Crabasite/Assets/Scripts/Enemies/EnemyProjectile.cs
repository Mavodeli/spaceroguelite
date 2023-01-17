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

<<<<<<<< HEAD:Crabasite/Assets/Scripts/Enemies/EnemyProjectile.cs
    public void Setup(GameObject _parent, 
                        GameObject _target, 
                        Sprite _sprite, 
                        float textureScale,
                        float _speed,
                        collision_delegate _delegate
                        ){
========
    public void Setup(GameObject _parent, GameObject _target, Sprite _sprite){
        msd = Resources.Load<MantisShrimpData>("Scriptable Objects/EnemyData/MantisShrimpData");

        //set hierarchy and world position
>>>>>>>> save-and-load-system:Crabasite/Assets/Scripts/Enemies/MantisShrimpSpear.cs
        parent = _parent;
        speed = _speed;
        del = _delegate;

        //position + direction
        gameObject.transform.parent = parent.transform;
        gameObject.transform.position = parent.transform.position;

        //determine facing direction
        targetDirection = (_target.transform.position-gameObject.transform.position).normalized;

        //rotation
        float angle = 90 + Vector3.Angle(new Vector3(0, 1, 0), targetDirection);
<<<<<<<< HEAD:Crabasite/Assets/Scripts/Enemies/EnemyProjectile.cs
        if(Mathf.Sign((gameObject.transform.position - _target.transform.position).x) == 1)//flip Spear correctly
            angle += 180;
        gameObject.transform.Rotate(new Vector3(0, 0, angle), Space.Self);

        gameObject.name = parent.name+" Projectile";
========
        gameObject.transform.Rotate(new Vector3(0, 0, angle));
        if(Mathf.Sign((gameObject.transform.position - _target.transform.position).x) == 1) {//flip Spear correctly
            gameObject.transform.Rotate(new Vector3(0, 0, 180));
        }

        //set gameobject properties
        gameObject.name = "Mantis Shrimp Spear";
>>>>>>>> save-and-load-system:Crabasite/Assets/Scripts/Enemies/MantisShrimpSpear.cs
        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Raycast");

        //setup SpriteRenderer
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        sr.drawMode = SpriteDrawMode.Sliced;//needed for scaling the sprite
        sr.size *= textureScale;
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
        bc.isTrigger = true;
    }

    void Update(){
        rb.AddForce(targetDirection*speed*Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other){
        if((other.gameObject != parent) && (other.gameObject.name != gameObject.name)){
            del(other);
            GameObject.Destroy(gameObject);
        }
    }
}