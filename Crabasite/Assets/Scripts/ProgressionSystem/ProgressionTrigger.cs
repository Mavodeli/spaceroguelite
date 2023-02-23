using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionTrigger : ProgressionParentClass
{
    public OnTriggerEnterDelegate del;

    /// <summary>
    /// Setup function for the Progression Trigger. Sets the gameObjects' Collider2D to be a trigger. 
    /// </summary>
    /// <param name="_delegate">The function that should be executed OnTriggerEnter. Pass it as a lambda method using 'delegate' (only void functions with no arguments are supported!).</param>
    public void Setup(OnTriggerEnterDelegate _delegate)
    {
        del = _delegate;
    }

    public void Start() {
        //setup Rigidbody2D
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //setup Collider2D
        Collider2D col = gameObject.GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            del();
        }
    }
}
