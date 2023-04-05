using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crush", menuName = "Ultimate/Create Crush")]
public class Crush : Ultimate
{
    // public LayerMask layermask;
    private float radius = 4;
    private float speed = 500;

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
            if (hitCollider.gameObject && hitCollider.gameObject.GetComponent<Rigidbody2D>())
            {
                AttractTwoBehaviour object1 =
                    hitCollider.gameObject.AddComponent<AttractTwoBehaviour>();
                object1.target = player.gameObject;
                object1.speed = speed;
                object1.DelayedDestroyOperation(0.5f);
            }
        }

        foreach (var hitCollider in hitColliders)
        {
            hitCollider.SendMessage("addHealth", -5, SendMessageOptions.DontRequireReceiver);
            hitCollider.SendMessage("addHealthToEmergencyDoor", -101, SendMessageOptions.DontRequireReceiver);
            hitCollider.SendMessage("addHealthToFalseWall", -101, SendMessageOptions.DontRequireReceiver);
        }
        
        isActive = false;
    }
}
