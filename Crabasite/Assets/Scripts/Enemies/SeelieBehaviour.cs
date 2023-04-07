using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SeelieBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxDistanceToPlayer;
    private Rigidbody2D rb;
    private Seeker seeker;
    private Animator animator;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Path path;
    private int currentWaypoint;
    private float nextWaypointDistance;
    private GameObject player;
    [SerializeField] private Vector3 destination;
    private TimerObject delayed_despawn;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        bc.size = new Vector2(.1f, .1f);
        delayed_despawn = new TimerObject(gameObject.name+" delayed_despawn", true);
        delayed_despawn.setOnRunningOut(delegate(){despawn();});

        seeker = gameObject.GetComponent<Seeker>();
        currentWaypoint = 0;
        nextWaypointDistance = 3.0f;
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(UpdateCoroutine(.5f));
    }

    private IEnumerator UpdateCoroutine(float UpdateRate){
        WaitForSeconds wait = new WaitForSeconds(UpdateRate);
        while(true){
            UpdatePath();
            yield return wait;
        }
    }

    private void UpdatePath(){
        seeker.StartPath(gameObject.transform.position, destination, OnPathComplete);
    }

    private void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        //init
        Vector3 force, direction = Vector3.zero;

        float distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
        float distanceToDestination = Vector3.Distance(gameObject.transform.position, destination);

        //move when near player
        if (distanceToPlayer <= maxDistanceToPlayer)
        {
            if (path == null) return;
            if (currentWaypoint >= path.vectorPath.Count) return;

            //dont move if destination reached
            if(distanceToDestination <= 1.2f){

                //fix overshooting destination
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                //initiate delayed despawn
                delayed_despawn.start(4);

                return;
            }

            //actual movement
            direction = path.vectorPath[currentWaypoint] - gameObject.transform.position;
            direction.Normalize();
            if (Vector2.Distance(gameObject.transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                currentWaypoint++;
        }
        else//dont move when not near player
        {
            haltMovement(direction);
            return;
        }

        force = direction * speed;
        // Debug.Log(force);
        rb.AddForce(force);

        //dampen velocity at high acceleration
        float veloX = Mathf.Clamp(rb.velocity.x, -speed, speed);
        float veloY = Mathf.Clamp(rb.velocity.y, -speed, speed);
        rb.velocity = new Vector2(veloX, veloY);
    }

    private void haltMovement(Vector3 direction){
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0.1f;
        direction = Vector3.zero;
    }

    private void despawn(){
        GameObject GH = GameObject.FindGameObjectWithTag("GameHandler");
        object[] args = new object[]{"NegativeChargeOrb", transform.position};
        GH.SendMessage("spawnItem", args, SendMessageOptions.DontRequireReceiver);
        Debug.Log("Puff!");
        Destroy(gameObject);
    }
}
