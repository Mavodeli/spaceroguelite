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
        Transform healtBarTransform = Instantiate(pfHealthbar, new Vector3(0,0), Quaternion.identity); 
        HealthBar healthBar = healtBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(_enemyHealth);
        healthBar.transform.parent = enemyTransform;
        healthBar.transform.localPosition = healthbarPosition;
    }
    // accessed damage function of the healthsystem
    private void EnemyTakeDmg(int dmg){
        _enemyHealth.Damage(dmg);
        //Debug.Log(_enemyHealth.Health);
    }
}
