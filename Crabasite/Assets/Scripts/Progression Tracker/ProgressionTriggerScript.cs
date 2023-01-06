using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionTriggerScript : MonoBehaviour
{
    private bool condition;
    public delegate void Function();
    // public static event Function addFunction;
    private Function function;

    /// <summary>
    /// Setup function for the Progression Trigger. Sets the gameObjects' BoxCollider2D to be a trigger (only if the collider exists). 
    /// </summary>
    /// <param name="_function">The function that should be executed OnTriggerEnter. Pass it as a lambda method using 'delegate' (only void functions with no arguments are supported!).</param>
    public void Setup(Function _function)
    {
        if(gameObject.GetComponent<BoxCollider2D>() != null){
            function = _function;
            BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
            bc.isTrigger = true;
        }
    }

    private void OnTriggerEnter(){
        function();
    }
}
