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
        // scale to be 1.5 times the size of the object (roughly)
        Bounds bounds;
        Renderer r = gameObject.GetComponent<Renderer>();
        if (r == null)
        {
            // Are there any valid children?
            Renderer[] childRenderers = gameObject.GetComponentsInChildren<Renderer>();
            if (childRenderers.Length < 1)
            {
                // if there aren't we default to 2.0f (size is always double the extents)
                bounds = new Bounds(Vector3.zero, new Vector3(1, 1, 1));
            }
            else
            {
                // if there are we calculate bounds from them:
                bounds = GetBoundsFromActiveChildren(gameObject, childRenderers);
            }
        }
        else
        {
            bounds = r.bounds;
        }
        // incorporate scale of object
        float scale = (transform.lossyScale.x + transform.lossyScale.y) / 2;
        repelRange.radius = (1.5f * Mathf.Max(bounds.size.x, bounds.size.y)) / scale;
        // attach particles (first getting the prefab around a few corners)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        particleObject = Transform.Instantiate(
            player.GetComponent<UltimateHolder>().NegativeChargeAnimationPrefab
        );
        particleObject.transform.SetParent(transform, false);
        // Change the layer of the gameobject to prevent duplicates and raycast intersecting with circlecollider
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void Update()
    {
        //check if the duration is over, if it is remove the sphere collider and the script
        chargeDuration -= Time.deltaTime;

        if (chargeDuration <= 0)
        {
            // Restore Layer
            gameObject.layer = LayerMask.NameToLayer("Raycast");
            // the particles use a custom destroy that lets particles play out
            if (particleObject)
            {
                particleObject.GetComponent<NegativeChargeParticleScript>().DestroyParticleObject();
            }
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

    Bounds GetBoundsFromActiveChildren(GameObject parent, Renderer[] childRenderers)
    {
        if (childRenderers.Length == 1)
        {
            // if only one child is valid, copy its bounds
            return childRenderers[0].bounds;
        }
        else
        {
            // otherwise combine bounds of all children
            Bounds bounds = childRenderers[0].bounds;
            for (int i = 1; i < childRenderers.Length; i++)
            {
                GameObject childObject = childRenderers[i].transform.gameObject;
                if (
                    childObject.activeSelf
                    && !childObject.GetComponent<NegativeChargeParticleScript>()
                )
                {
                    bounds.Encapsulate(childRenderers[i].bounds);
                }
            }
            return bounds;
        }
    }
}
