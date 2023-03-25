using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EmergencyDoorBehaviour : MonoBehaviour, IDataPersistence
{
    private bool unlocked;
    private TimerObject isOpen_timer;
    private DataPersistenceManager dpm;
    private HealthSystem HS;
    private BoxCollider2D bc;
    private InteractionButton ib;

    void Start(){
        dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        unlocked = dpm.getGameData().AS_EmergencyDoorUnlocked;
        HS = new HealthSystem(100, 100);
        isOpen_timer = new TimerObject(gameObject.name+" isOpen_timer");
        isOpen_timer.setOnRunningOut(delegate(){close();});
        bc = gameObject.GetComponent<BoxCollider2D>();
        ib = gameObject.GetComponent<InteractionButton>();
        ib.Setup(delegate(){
            GameObject.FindGameObjectWithTag("QuestEventsContainer").SendMessage("InvokeEvent", "interactedWithEmergencyDoor", SendMessageOptions.DontRequireReceiver);
            if(unlocked) open();
        }, _otherDelegate: delegate(){
            InteractionButton.displayProtagonistComment(gameObject.name+"_IsNotInteractable");
            GameObject GH = GameObject.FindGameObjectWithTag("GameHandler");
            GH.SendMessage("addNewQuest", "OpenTheEmergencyDoor", SendMessageOptions.DontRequireReceiver);
        });
        ib.setNewOffset(new Vector3(0, 0, 0));
        ib.setInteractability(unlocked);
    }

    public void open(){
        if(unlocked && !isOpen_timer.runs()){
            isOpen_timer.start(5);
            bc.enabled = false;
            //TODO change sprite
        }
    }

    private void close(){
        if(unlocked){
            bc.enabled = true;
            //TODO change sprite
            if(isOpen_timer.runs()) isOpen_timer.stop();
        }
    }

    private void unlock(){
        // Debug.Log(gameObject.name+": I'm open now :)");
        unlocked = true;
        ib.setInteractability(true);
        InteractionButton.displayProtagonistComment(gameObject.name+"_JustGotUnlocked");
    }

    public bool isUnlocked(){
        return unlocked;
    }

    private void addHealthToEmergencyDoor(int hp){
        if(!unlocked){
            // Debug.Log(gameObject.name+": Help! I'm taking "+(-hp)+" damage!");
            float health = Mathf.Clamp(HS.Health+hp,0,HS.MaxHealth);
            if(hp < 0) HS.Damage(-hp);
            else HS.Heal(hp);
            if(health == 0) unlock();
        }
    }

    public void LoadData(GameData data)
    {
        unlocked = data.AS_EmergencyDoorUnlocked;
    }

    public void SaveData(ref GameData data)
    {
        data.AS_EmergencyDoorUnlocked = unlocked;
    }
}
