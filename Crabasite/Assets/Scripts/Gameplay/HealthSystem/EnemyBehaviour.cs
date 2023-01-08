using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public HealthSystem _enemyHealth = new HealthSystem(100,100);
    public Transform pfHealthbar;
    public Transform enemyTransform;
    public Vector3 healthbarPosition = new Vector3(0,.5f,0);

    // setup process for healthsystem
    // instantiate object from prefab and attach healthbar to the gameobject to which this script is attached to
    private void Start() {
        HealthBar healthBar = _enemyHealth.attachHealthBar(gameObject, healthbarPosition.y);
    }

    void Update(){
        if(_enemyHealth.Health == 0){
            Debug.Log(gameObject.name+" has 0 health and thus will be destroyed!");
            Destroy(gameObject);
        }
    }

    // accessed damage function of the healthsystem
    private void EnemyTakeDmg(int dmg){
        _enemyHealth.Damage(dmg);
        //Debug.Log(_enemyHealth.Health);
    }
}
