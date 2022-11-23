using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttractTwo : Ultimate
{
    GameObject selectedObject1;
    GameObject selectedObject2;

    CollisionDetector detectorObject1;
    CollisionDetector detectorObject2;

    Transform object1Transform;
    Transform object2Transform;

    Vector3 object1position;
    Vector3 object2position;

    public bool _initiated = false;

    public float object1Speed;
    public float object2Speed;

    const int SPEED = 5;

    //pulls two selected targets together. Currently automatically selects the GameObject and GameObject2
    //TODO: make targets selectable.
    //TODO: make it do dmg
    public override void Use()
    {
        if (!_initiated)
        {
            // (re)set the speed
            object1Speed = SPEED;
            object2Speed = SPEED;

            selectedObject1 = GameObject.Find("GameObject");
            selectedObject2 = GameObject.Find("GameObject2");

            //attach Colliding Detector Scripts to the selected Objects
            selectedObject1.AddComponent<CollisionDetector>();
            selectedObject2.AddComponent<CollisionDetector>();

            //get references to the script
            detectorObject1 = selectedObject1.GetComponent<CollisionDetector>();
            detectorObject2 = selectedObject2.GetComponent<CollisionDetector>();

            //get transforms of the objects
            object1Transform = selectedObject1.GetComponent<Transform>();
            object2Transform = selectedObject2.GetComponent<Transform>();

            object1position = object1Transform.position;
            object2position = object2Transform.position;

            _initiated = true;
        }

        //exit condition
        //check if both objects have collided with something and stop the ultimate
        if (detectorObject1.isColliding && detectorObject2.isColliding)
        {
            isActive = false;
            _initiated = false;
            object1Speed = SPEED;
            object2Speed = SPEED;
        } 


        // if either object has collided, make it's speed 0
        // has to be seperate since objects can collide at different times
        if (detectorObject1.isColliding)
        {
            object1Speed = 0;
        }

        if (detectorObject2.isColliding)
        {
            object2Speed = 0;
        }

        //move towards the other object
        object2Transform.position = Vector3.MoveTowards(object2Transform.position, object1position, object2Speed * Time.deltaTime);
        object1Transform.position = Vector3.MoveTowards(object1Transform.position, object2position, object1Speed * Time.deltaTime);
    }
}
