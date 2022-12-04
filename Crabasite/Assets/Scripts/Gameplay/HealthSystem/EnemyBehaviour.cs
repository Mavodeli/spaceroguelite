using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public HealthSystem _enemyHealth = new HealthSystem(100,100);

    private void EnemyTakeDmg(int dmg){
        _enemyHealth.Damage(dmg);
        Debug.Log(_enemyHealth.Health);
    }
}
