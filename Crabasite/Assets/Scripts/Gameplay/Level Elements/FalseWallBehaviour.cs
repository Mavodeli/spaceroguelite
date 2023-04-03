using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FalseWallBehaviour : MonoBehaviour
{
    private TimerObject isOpen_timer;
    private HealthSystem HS;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Sprite closedSprite;
    private Sprite openSprite;

    void Start(){
        HS = new HealthSystem(100, 100);

        isOpen_timer = new TimerObject(gameObject.name+" isOpen_timer");
        isOpen_timer.setOnRunningOut(delegate(){close();});

        bc = gameObject.GetComponent<BoxCollider2D>();

        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void open(){
        if(!isOpen_timer.runs()){
            isOpen_timer.start(5);
            bc.enabled = false;
            sr.color *= new Color(1, 1, 1, .65f);
        }
    }

    private void close(){
        bc.enabled = true;
        sr.color = new Color(1, 1, 1, 1);
        HS.Heal(100);
        if(isOpen_timer.runs()) isOpen_timer.stop();
    }

    private void addHealthToFalseWall(int hp){
        float health = Mathf.Clamp(HS.Health+hp,0,HS.MaxHealth);
        if(hp < 0) HS.Damage(-hp);
        else HS.Heal(hp);
        if(health == 0) open();
    }
}
