using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 100;
    private int maxhealth = 100;
    private HealthSystem HS;
    private HealthBar healthBar;
    
    void Start()
    {
        HS = new HealthSystem(health, maxhealth);
        float ySize = gameObject.GetComponent<SpriteRenderer>().size.y;
        healthBar = HS.attachHealthBar(gameObject, ySize/2+.5f);
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
            Debug.Log("Player.health == 0 => Game Over");
            //TODO: some kind of Game Over
        }
    }

    public bool isAlive(){
        return health > 0;
    }
}
