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
        GameObject pfManaBar = Resources.Load<GameObject>("Prefabs/ManaSystem/pfPlayerManaBar");
        Transform manaBarTransform = Transform.Instantiate(pfManaBar.transform, Vector3.zero, Quaternion.identity);
        ManaBar manaBar = manaBarTransform.GetComponent<ManaBar>();
        manaBar.Setup(MS);
        manaBar.transform.parent = GameObject.FindGameObjectWithTag("HUD").transform;
        manaBar.name = "PlayerMana";
        manaBar.transform.localPosition = new Vector3(0, -25, 0);
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
