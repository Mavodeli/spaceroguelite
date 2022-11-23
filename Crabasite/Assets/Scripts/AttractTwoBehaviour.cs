using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractTwoBehaviour : MonoBehaviour
{
    //target that will be pulled towards
    public GameObject target;
    //speed of the pull
    public float speed;

    //Collision detection script
    CollisionDetector detector;
 
    Transform myTransform;  
    Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        //Initializes fields
        detector = target.AddComponent<CollisionDetector>();
        myTransform = gameObject.GetComponent<Transform>();
        targetTransform = target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the Collision detector detects a collision with anything, the pull script will be destroyed to stop the pull
        if (detector.isColliding)
        {
            Destroy(GetComponent<AttractTwoBehaviour>());
        }

        //pulls the object towards the target
        myTransform.position = Vector3.MoveTowards(myTransform.position, targetTransform.position, speed * Time.deltaTime);
    }

}
