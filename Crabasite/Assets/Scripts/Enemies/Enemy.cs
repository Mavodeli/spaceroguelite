using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private HealthSystem HS;
    private HealthBar healthBar;
    private float health;
    private float maxhealth;
    private float damage;
    private float speed;
    private string _name;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public void initialSetup(float _health,
                                float _maxhealth, 
                                float _damage,
                                float _speed, 
                                string __name,
                                Sprite _sprite, 
                                float spriteScale
                                )
    {
        //setup properties
        health = _health;
        maxhealth = _maxhealth;
        damage = _damage;
        _name = __name;
        speed = _speed;

        //setup name, tag & layer
        gameObject.name = _name;
        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Enemies");

        //setup SpriteRenderer
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        sr.drawMode = SpriteDrawMode.Sliced;//needed for scaling the sprite
        sr.size *= spriteScale;
        sr.sortingOrder = 1;

        //setup HealthSystem
        HS = new HealthSystem((int) health, (int) maxhealth);
        HealthBar healthBar = HS.attachHealthBar(gameObject, sr.size.y/2+.1f);

        //setup Rigidbody2D
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //setup BoxCollider2D
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = sr.size;
    }

    //negative hp damages, positive hp heals
    public void addHealth(int hp){
        health = Mathf.Clamp(health+hp,0,maxhealth);
        if(hp < 0){
            HS.Damage(-hp);
        }
        else{
            HS.Heal(hp);
        }
        
        if(health == 0){
            Debug.Log("You killed a "+_name+"!");
            Destroy(gameObject);
        }
    }

    public Rigidbody2D getRigidbody(){
        return rb;
    }

    public BoxCollider2D getCollider(){
        return bc;
    }

    public float getSpeed(){
        return speed;
    }
    public float getDamage(){
        return damage;
    }
}
