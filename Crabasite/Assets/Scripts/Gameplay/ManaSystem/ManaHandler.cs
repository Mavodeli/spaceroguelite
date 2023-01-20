using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaHandler : MonoBehaviour
{
    public static ManaHandler manaHandler {get; private set;}
    // create manaSystem, can also be created in relevant script
    public ManaSystem _playerMana = new ManaSystem(1000,1000);

    void Awake() {
        if(manaHandler != null && manaHandler != this){
            Destroy(this);
        }    
        else{
            manaHandler = this;
        }
    }
}
