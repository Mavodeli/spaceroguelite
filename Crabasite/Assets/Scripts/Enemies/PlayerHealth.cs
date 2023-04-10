using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    private int health = 100;
    private int maxhealth = 100;
    private HealthSystem HS;
    public GameOverScreen GameOverScreen;
    public bool invincible = false;

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

        addHealth(0);
    }

    //negative hp damages, positive hp heals
    public void addHealth(int hp)
    {
        if (invincible && hp < 0) // prevent damage without preventing healing
        {
            hp = 0;
        }
        health = Mathf.Clamp(health + hp, 0, maxhealth);
        if (hp < 0)
        {
            HS.Damage(-hp);
        }
        else
        {
            HS.Heal(hp);
        }

        if (!isAlive())
        {
            CommentarySystem.abortDisplayingComment();
            Debug.Log("YOU DIED");
            DataPersistenceManager.instance.LoadGame(true);
            GameOverScreen.Setup();
        }
    }

    public bool isAlive()
    {
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

    public IEnumerator makeInvincible(float duration)
    {
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
    }
}
