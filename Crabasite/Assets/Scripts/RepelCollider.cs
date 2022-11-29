using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelCollider : MonoBehaviour
{
    //range in which the force of the negative charge will start affecting other objects
    public CircleCollider2D repelRange;
    //duration of the negative charge
    float chargeDuration = 5f;

    private void Start()
    {
        //initiate the sphere collider that will act as the negative charge range
        repelRange = gameObject.AddComponent<CircleCollider2D>();
        Debug.Log(repelRange);
        //isTrigger will make it so it does not cause collision
        repelRange.isTrigger = true;
        repelRange.radius = 1.5f;
    }

    private void Update()
    {
        //check if the duration is over, if it is remove the sphere collider and the script
        chargeDuration -= Time.deltaTime;

        if (chargeDuration <= 0)
        {
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
