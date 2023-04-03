using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelCollider : MonoBehaviour
{
    //range in which the force of the negative charge will start affecting other objects
    public CircleCollider2D repelRange;

    //duration of the negative charge
    float chargeDuration = 5.0f;

    // particle animation child object
    GameObject particleObject;

    private void Start()
    {
        //initiate the sphere collider that will act as the negative charge range
        repelRange = gameObject.AddComponent<CircleCollider2D>();
        //isTrigger will make it so it does not cause collision
        repelRange.isTrigger = true;
        // scale to be 2 times the size of the object (roughly)
        Renderer r = gameObject.GetComponent<Renderer>();
        repelRange.radius = 2.0f * Mathf.Max(r.bounds.size.x, r.bounds.size.y);
        // attach particles (first getting the prefab around a few corners)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        particleObject = Transform.Instantiate(
            player.GetComponent<UltimateHolder>().NegativeChargeAnimationPrefab
        );
        particleObject.transform.SetParent(gameObject.transform, false);
    }

    private void Update()
    {
        //check if the duration is over, if it is remove the sphere collider and the script
        chargeDuration -= Time.deltaTime;

        if (chargeDuration <= 0)
        {
            Destroy(particleObject);
            Destroy(GetComponent<CircleCollider2D>());
            Destroy(GetComponent<RepelCollider>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if an object enters the sphere collider radius, add a repelbehaviour script to the object to make it repel
        GameObject collidingObject = collision.gameObject;
        RepelBehaviour repelScript = collidingObject.AddComponent<RepelBehaviour>();

        repelScript.infusedObject = gameObject;
    }
}
