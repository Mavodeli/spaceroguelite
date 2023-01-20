using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour, IDataPersistence
{
    private int mana = 100;
    private int maxMana = 100;
    private ManaSystem MS;
    private ManaBar manaBar;
    void Start()
    {
        MS = new ManaSystem(mana, maxMana);
        float ySize = gameObject.GetComponent<SpriteRenderer>().size.y;
        manaBar = MS.attachManaBar(gameObject, ySize/3+0.7f);
    }

    //negative mp uses, positive mp refills
    public void addMana(int mp){
        mana = Mathf.Clamp(mana+mp,0,maxMana);
        if(mp < 0){
            MS.ManaUsage(-mp);
        }
        else{
            MS.ManaRefill(mp);
        }
        
        if(mana == 0){
            Debug.Log("Player.mana == 0 => No Mana left");
        }
    }

    public bool hasMana(){
        return mana > 0;
    }

     public void LoadData(GameData data)
    {
        this.mana = data.mana;
    }
    public void SaveData(ref GameData data)
    {
        data.mana = this.mana;
    }
}
