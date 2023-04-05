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
    [SerializeField] private float stoppingDistance;
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
        //movement
        Vector3 force, direction;
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= maxDistanceToPlayer)
        {
            if (path == null) return;
            if (currentWaypoint >= path.vectorPath.Count) return;
            direction = path.vectorPath[currentWaypoint] - gameObject.transform.position;
            direction.Normalize();
            if (Vector2.Distance(gameObject.transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                currentWaypoint++;
            
            if(Vector3.Distance(gameObject.transform.position, destination) <= maxDistanceToPlayer)
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
            direction = Vector3.zero;
        }
        force = direction * speed;
        rb.AddForce(force);
    }
}
