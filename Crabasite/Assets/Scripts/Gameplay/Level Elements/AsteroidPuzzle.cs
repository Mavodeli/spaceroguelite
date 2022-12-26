using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPuzzle : MonoBehaviour
{
    private GameObject collidedWith;

    public GameObject player;
    public GameObject self;
    public int requiredForce;

    // trigger event from inspector for debugging
    public bool collided;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != player & collision.relativeVelocity.magnitude > requiredForce)
        {
            collidedWith = collision.gameObject;
            collided = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (collided)
        {
            Destroy(collidedWith);
            Destroy(self);
        }
    }
}
