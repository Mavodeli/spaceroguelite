using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    private int health = 100;
    private int maxhealth = 100;
    private HealthSystem HS;
    public GameOverScreen GameOverScreen;

    void Start()
    {
        HS = new HealthSystem(health, maxhealth);
        GameObject pfHealthBar = Resources.Load<GameObject>("Prefabs/HealthSystem/pfPlayerHealthBar");
        Transform healthBarTransform = Transform.Instantiate(pfHealthBar.transform, Vector3.zero, Quaternion.identity);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(HS);

        healthBar.transform.parent = GameObject.FindGameObjectWithTag("HUD").transform;
        healthBar.name = "PlayerHealth";
        healthBar.transform.localPosition = new Vector3(0, 0, 0);
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
        
        if(!isAlive()){
            Debug.Log("YOU DIED");
            DataPersistenceManager.instance.LoadGame();
            GameOverScreen.Setup();
        }
    }

    public bool isAlive(){
        return health > 0;
    }

     public void LoadData(GameData data)
    {
        this.health = data.health;
    }
    public void SaveData(ref GameData data)
    {
        data.health = this.health;
    }
}
