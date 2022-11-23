using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple script to attach to an object to detect collision
public class CollisionDetector : MonoBehaviour
{
    public bool isColliding;

    private void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}
