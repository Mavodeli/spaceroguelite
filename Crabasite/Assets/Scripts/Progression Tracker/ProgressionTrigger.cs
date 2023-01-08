using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionTrigger : ProgressionDelegate
{
    public Function function;

    /// <summary>
    /// Setup function for the Progression Trigger. Sets the gameObjects' BoxCollider2D to be a trigger. 
    /// </summary>
    /// <param name="_function">The function that should be executed OnTriggerEnter. Pass it as a lambda method using 'delegate' (only void functions with no arguments are supported!).</param>
    public void Setup(Function _function)
    {
        function = _function;

        //setup Rigidbody2D
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //setup BoxCollider2D
        BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            function();
        }
    }
}
