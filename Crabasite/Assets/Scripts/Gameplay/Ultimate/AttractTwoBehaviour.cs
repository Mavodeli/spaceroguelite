using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractTwoBehaviour : MonoBehaviour
{
    //target that will be pulled towards
    public GameObject target;
    //speed of the pull
    public float speed;
    Rigidbody2D myRigidbody;
    float _force;

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
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _force = 5;
    }

    // Update is called once per frame
    void Update()
    {
        //if the Collision detector detects a collision with anything, the pull script will be destroyed to stop the pull
        if (detector.isColliding)
        {
            gameObject.GetComponent<BoxCollider2D>().SendMessage("EnemyTakeDmg", 5*_force, SendMessageOptions.DontRequireReceiver);
            Destroy(GetComponent<AttractTwoBehaviour>());
        }

        //pulls the object towards the target
        //myTransform.position = Vector3.MoveTowards(myTransform.position, targetTransform.position, speed * Time.deltaTime);
        Vector3 gameObjectToTargetDirection = new Vector3(target.transform.position.x - gameObject.transform.position.x, target.transform.position.y - gameObject.transform.position.y, 0);
        gameObjectToTargetDirection.Normalize();

        myRigidbody.AddForce(gameObjectToTargetDirection * _force);
    }

}
