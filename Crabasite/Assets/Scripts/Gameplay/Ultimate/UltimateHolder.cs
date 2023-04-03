using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateHolder : MonoBehaviour, IDataPersistence
{
    public Ultimate ultimate;
    private ScriptableObject[] ultimateList; // array for all ultimates

    private int equippedUltimate;

    // timer for remaining cooldown
    public float cooldownTimer;
    public Transform player;

    enum UltimateState
    {
        ready,
        active,
        cooldown
    }

    UltimateState state;

    // key can be assiged in inspector
    public KeyCode key;

    // Particle Animations to hand over to specific ultimate
    public GameObject CrushAnimationPrefab;
    public GameObject NegativeChargeAnimationPrefab;

    // Start is called before the first frame update
    void Start()
    {
        state = UltimateState.ready;
        ultimateList = Resources.LoadAll<ScriptableObject>("ScriptableObjects/Ultimates");
        SwitchUltimate(equippedUltimate);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case UltimateState.ready:
                if (Input.GetKeyDown(key))
                {
                    ultimate.isActive = true;
                    state = UltimateState.active;
                }
                break;
            case UltimateState.active:
                // if ult has finished, change state to cooldown
                if (!ultimate.isActive)
                {                   
                    state = UltimateState.cooldown;
                    cooldownTimer = ultimate.cooldown;
                } else
                {
                    ultimate.Use();
                }
                break;
            case UltimateState.cooldown:
                // change state once cooldown is over
                if (cooldownTimer < 0)
                {
                    state = UltimateState.ready;
                } else
                {
                    cooldownTimer -= Time.deltaTime;
                }
                break;
        }
    }

    public void SwitchUltimate(int ult)
    {
        DataPersistenceManager dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        GameObject hud = GameObject.FindGameObjectWithTag("HUD");

        if(ult == 3){//empty
            ultimate = (Ultimate)ultimateList[ult];
            hud.SendMessage("ChangeSprite", ult, SendMessageOptions.DontRequireReceiver);
            equippedUltimate = ult;
            return;
        }

        bool unlocked = dpm.getGameData().UltimateDict[ult];
        if(unlocked){
            ultimate = (Ultimate)ultimateList[ult];
            if(ult == 1) { ultimate.player = player; }
            hud.SendMessage("ChangeSprite", ult, SendMessageOptions.DontRequireReceiver);
            equippedUltimate = ult;
        }
    }

    public void LoadData(GameData data)
    {
        equippedUltimate = data.lastEquippedUltimate;
    }
    
    public void SaveData(ref GameData data)
    {
        data.lastEquippedUltimate = equippedUltimate;
    }
}
