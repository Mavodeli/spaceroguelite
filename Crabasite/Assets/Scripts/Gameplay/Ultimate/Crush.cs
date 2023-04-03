using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crush", menuName = "Ultimate/Create Crush")]
public class Crush : Ultimate
{
    // public LayerMask layermask;
    public int radius;
    public float speed = 5;

    public override void Use()
    {
        // play animation
        GameObject animPrefab = player.gameObject
            .GetComponent<UltimateHolder>()
            .CrushAnimationPrefab;
        GameObject anim = Transform.Instantiate(animPrefab);
        anim.transform.parent = player;

        Vector3 center = player.position;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            center,
            radius,
            LayerMask.GetMask("Raycast")
        );

        foreach (var hitCollider in hitColliders)
        {
            AttractTwoBehaviour object1 =
                hitCollider.gameObject.AddComponent<AttractTwoBehaviour>();
            object1.target = player.gameObject;
            object1.speed = speed;
        }

        foreach (var hitCollider in hitColliders)
        {
            // hitCollider.SendMessage("EnemyTakeDmg", 5, SendMessageOptions.DontRequireReceiver);
            hitCollider.SendMessage("addHealth", -5, SendMessageOptions.DontRequireReceiver);
            hitCollider.SendMessage(
                "addHealthToEmergencyDoor",
                -34,
                SendMessageOptions.DontRequireReceiver
            );
            // Debug.Log(hitCollider);
        }

        isActive = false;
    }
}
