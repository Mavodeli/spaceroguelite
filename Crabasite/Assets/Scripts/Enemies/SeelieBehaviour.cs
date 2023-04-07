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

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        bc.size = sr.size;

        seeker = gameObject.GetComponent<Seeker>();
        currentWaypoint = 0;
        nextWaypointDistance = 3.0f;
        player = GameObject.FindGameObjectWithTag("Player");

        destination = GameObject.Find("TheCure").transform.position;

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

        Debug.Log(Vector3.Distance(gameObject.transform.position, destination));

        //move when near player
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= maxDistanceToPlayer)
        {
            if (path == null) return;
            if (currentWaypoint >= path.vectorPath.Count) return;

            //dont move if destination reached
            if(Vector3.Distance(gameObject.transform.position, destination) <= maxDistanceToPlayer){
                //fix overshooting destination
                haltMovement(direction);
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

        // randomizeDirection(direction, 3);
        force = direction * speed;
        // Debug.Log(force);
        rb.AddForce(force);
    }

    private void haltMovement(Vector3 direction){
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0.1f;
        direction = Vector3.zero;
    }

    private void randomizeDirection(Vector3 direction, float factor){
        float rndX = Random.Range(-1, 1);
        float rndY = Random.Range(-1, 1);
        direction += new Vector3(rndX, rndY, 0)*factor;
    }
}
