using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassWallBehaviour : MonoBehaviour
{
    private SpriteRenderer sr;
    private Sprite intactSprite;
    private Sprite brokenSprite;
    private BoxCollider2D bc;
    private DataPersistenceManager dpm;
    private HealthSystem HS;
    private bool isIntact;


    void Start(){
        dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        isIntact = dpm.getGameData().AS_GlassWallIntact;

        HS = new HealthSystem(1, 1);

        sr = gameObject.GetComponent<SpriteRenderer>();
        intactSprite = Resources.Load<Sprite>("Sprites/StationaryObjects/glass_intact");
        brokenSprite = Resources.Load<Sprite>("Sprites/StationaryObjects/glass_broken");
        if(isIntact) sr.sprite = intactSprite;
        else sr.sprite = brokenSprite;

        bc = gameObject.GetComponent<BoxCollider2D>();
        bc.enabled = isIntact;
    }

    private void breakGlassWall(){
        isIntact = false;
        dpm.getGameData().AS_GlassWallIntact = false;
        bc.enabled = false;
        sr.sprite = brokenSprite;
        transform.localScale = new Vector3(0, 0, 0);
    }

    private void addHealthToGlassWall(int hp){
        if(isIntact){
            float health = Mathf.Clamp(HS.Health+hp,0,HS.MaxHealth);
            if(hp < 0) HS.Damage(-hp);
            else HS.Heal(hp);
            if(health == 0) breakGlassWall();
        }
    }
}
