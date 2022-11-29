using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple script to attach to an object to detect collision
public class CollisionDetector : MonoBehaviour
{
    public bool isColliding;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }
}
