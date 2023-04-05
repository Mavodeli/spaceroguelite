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
    Vector3 gameObjectToTargetDirection;

    //Collision detection script
    CollisionDetector detector;

    Transform myTransform;
    Transform targetTransform;

    // particle child object
    GameObject particleObject;

    // Start is called before the first frame update
    void Start()
    {
        //Initializes fields
        myTransform = gameObject.GetComponent<Transform>();
        targetTransform = target.GetComponent<Transform>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        // if we don't have a rigidbody, our parent should have one
        if (!myRigidbody)
        {
            myRigidbody = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        }

        //set collision detection to continuous
        myRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // attach particle object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        particleObject = Transform.Instantiate(
            player.GetComponent<UltimateHolder>().AttractTwoAnimationPrefab
        );
        particleObject.GetComponent<AttractTwoParticleScript>().SetTarget(target);
        particleObject.transform.SetParent(transform, false);
        // Ignore Raycasts while active
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    // Update is called once per frame
    void Update()
    {
        // check if our target is still valid and stop if not
        if (!target || !target.transform || !target.activeSelf)
        {
            DestroyOperation();
        }
        //pulls the object towards the target
        //myTransform.position = Vector3.MoveTowards(myTransform.position, targetTransform.position, speed * Time.deltaTime);
        gameObjectToTargetDirection = new Vector3(
            target.transform.position.x - gameObject.transform.position.x,
            target.transform.position.y - gameObject.transform.position.y,
            0
        );
        gameObjectToTargetDirection.Normalize();

        myRigidbody.AddForce(gameObjectToTargetDirection * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the Collision detector detects a collision with anything, the pull script will be destroyed to stop the pull
        float damageToDeal = -0.02f * lossyCollisionForce(collision);
        Debug.Log(damageToDeal);
        gameObject.SendMessage("addHealth", damageToDeal, SendMessageOptions.DontRequireReceiver);
        collision.gameObject.SendMessage("addHealth", damageToDeal, SendMessageOptions.DontRequireReceiver);
        // we deal damage to both ourselves and the object we collided with
        // this way damage is dealt even when colliding with an obstacle inbetween
        // a force of 5000 deals 100 damage -> if we collide with our target 2500 each deals 100 damage. 2500 is also the amount of force required to break the silicate asteroids

        gameObject.SendMessage("addHealthToGlassWall", -1, SendMessageOptions.DontRequireReceiver);

        //return collision detection to discrete
        myRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        //remove pull force
        myRigidbody.velocity = new Vector2(0, 0);
        myRigidbody.angularVelocity = 0;
        //slightly randomize bounce direction
        float randomAngle = Random.Range(-4.0f, 4.0f);
        Vector3 randomizedBounceDirection =
            Quaternion.Euler(0, 0, randomAngle) * -gameObjectToTargetDirection;
        // bounce off collision
        myRigidbody.AddForce(randomizedBounceDirection * 4 * speed);
        // destroy
        DestroyOperation();
    }

    private void DestroyOperation()
    {
        // Restore Layer
        gameObject.layer = LayerMask.NameToLayer("Raycast");
        // Destroy operations
        if (particleObject)
        {
            particleObject.GetComponent<AttractTwoParticleScript>().DestroyParticleObject();
        }
        Destroy(this);
    }

    public void DelayedDestroyOperation(float time)
    {
        StartCoroutine(DelayedDestroyOperationCoroutine(time));
    }

    IEnumerator DelayedDestroyOperationCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        DestroyOperation();
    }

    public float lossyCollisionForce(Collision2D collision)
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        return Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity)
            * rigidbody.mass;
    }
}
