using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Crush : Ultimate
{
    public LayerMask layermask;
    public int radius;
    public float speed = 5;

    public override void Use()
    {
        Vector3 center = player.position;
        
    
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius, layermask);
        
        foreach (var hitCollider in hitColliders)
        {
            AttractTwoBehaviour object1 = hitCollider.gameObject.AddComponent<AttractTwoBehaviour>();
            object1.target = player.gameObject;
            object1.speed = speed;
        }

        foreach (var hitCollider in hitColliders)
        {
            hitCollider.SendMessage("EnemyTakeDmg", 5, SendMessageOptions.DontRequireReceiver);
            Debug.Log(hitCollider);
        }

        isActive = false;

    }

}
