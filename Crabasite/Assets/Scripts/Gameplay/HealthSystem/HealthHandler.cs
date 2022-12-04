using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public static HealthHandler healthHandler {get; private set;}

    public HealthSystem _playerHealth = new HealthSystem(100,100);
    public HealthSystem _enemyHealth = new HealthSystem(100,100);

    void Awake() {
        if(healthHandler != null && healthHandler != this){
            Destroy(this);
        }    
        else{
            healthHandler = this;
        }
    }
}
