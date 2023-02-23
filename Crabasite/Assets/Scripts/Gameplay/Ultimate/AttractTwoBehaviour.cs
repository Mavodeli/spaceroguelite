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
    Vector3 gameObjectToTargetDirection;

    //Collision detection script
    CollisionDetector detector;
 
    Transform myTransform;  
    Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        //Initializes fields
        myTransform = gameObject.GetComponent<Transform>();
        targetTransform = target.GetComponent<Transform>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _force = 5;

        //set collision detection to continuous
        myRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    // Update is called once per frame
    void Update()
    {
        //pulls the object towards the target
        //myTransform.position = Vector3.MoveTowards(myTransform.position, targetTransform.position, speed * Time.deltaTime);
        gameObjectToTargetDirection = new Vector3(target.transform.position.x - gameObject.transform.position.x, target.transform.position.y - gameObject.transform.position.y, 0);
        gameObjectToTargetDirection.Normalize();

        myRigidbody.AddForce(gameObjectToTargetDirection * _force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the Collision detector detects a collision with anything, the pull script will be destroyed to stop the pull
        if(gameObject.tag == "Collectable"){
            gameObject.GetComponent<Collider2D>().SendMessage("EnemyTakeDmg", 5*_force, SendMessageOptions.DontRequireReceiver);
        }
        if(gameObject.tag == "Enemy"){
            gameObject.GetComponent<Collider2D>().SendMessage("addHealth", -5*_force, SendMessageOptions.DontRequireReceiver);
        }
        //return collision detection to discrete
        myRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        //slightly randomize bounce direction
        float randomAngle = Random.Range(-4.0f, 4.0f);
        Vector3 randomizedBounceDirection = Quaternion.Euler(0, 0, randomAngle) * -gameObjectToTargetDirection;
        // bounce off collision
        myRigidbody.AddForce(randomizedBounceDirection * 4 * _force);
        Destroy(this);
    }
}
