using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    //HealthSystem
    private HealthSystem HS;
    private HealthBar healthBar;

    //Gameplay Properties
    private float health;
    private float maxhealth;
    private float damage;
    private float speed;
    private string _name;

    //Components
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Seeker seeker;
<<<<<<< Updated upstream
=======
    protected SpriteRenderer sr;
    protected BoxCollider2D bc;
>>>>>>> Stashed changes

    //melee damage
    private float meleeDamage;
    private float meleeDistance;
    private float meleeCooldown;
    private TimerObject bite_timer;

    //Misc
    private Path path;
    private int currentWaypoint;
    private float nextWaypointDistance;
    private float stoppingDistance;
    private GameObject player;
    private bool nearPlayer;

<<<<<<< Updated upstream
    public void initialSetup(float _health,
                                float _maxhealth, 
                                float _damage,
=======
    protected void initialSetup(float _health,
                                float _maxhealth,
>>>>>>> Stashed changes
                                float _meleeDamage,
                                float _meleeCooldown,
                                float _speed, 
                                string name,
                                Sprite _sprite, 
                                float spriteScale,
                                float _stoppingDistance
                                )
    {
        //setup properties
        health = _health;
        maxhealth = _maxhealth;
        damage = _damage;
        meleeDamage = _meleeDamage;
        meleeCooldown = _meleeCooldown;
        _name = name;
        speed = _speed;
        nearPlayer = false;

        //setup name, tag & layer
        gameObject.name = _name;
        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Raycast");

        //setup SpriteRenderer
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        sr.drawMode = SpriteDrawMode.Sliced;//needed for scaling the sprite
        sr.size *= spriteScale;
        sr.sortingOrder = 1;

        //setup HealthSystem
        HS = new HealthSystem((int) health, (int) maxhealth);
        HealthBar healthBar = HS.attachHealthBar(gameObject, sr.size.y/2+.1f);

        //setup Rigidbody2D
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/EnemyMaterial");

        //setup BoxCollider2D
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = sr.size;

        //setup Pathfinding
        seeker = gameObject.AddComponent<Seeker>();
        currentWaypoint = 0;
        nextWaypointDistance = 3.0f;
        stoppingDistance = _stoppingDistance;
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.AddComponent<DynamicGridObstacle>();

        //setup melee damage
        bite_timer = new TimerObject();
        meleeDistance = 
            (sr.size.x/2)+//~offset fish origin to collider edge
            (player.GetComponent<SpriteRenderer>().size.x/2)+//~offset player origin to collider edge
            .2f;//actual distance :)
    }

    private void Start(){
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
        seeker.StartPath(gameObject.transform.position, player.transform.position, OnPathComplete);
    }

    private void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update(){
        //movement
        Vector3 force;
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) >= stoppingDistance){
            if(path == null) return;
            if(currentWaypoint >= path.vectorPath.Count) return;
            Vector3 direction = path.vectorPath[currentWaypoint] - gameObject.transform.position;
            direction.Normalize();
            force = direction*speed*Time.deltaTime;
            if(Vector2.Distance(gameObject.transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                currentWaypoint++;
        }
        else{
            Vector3[] map = getNormalizedDirectionMap();
            Vector3 direction = map[Random.Range(0, map.Length-1)];
            force = direction*speed*Time.deltaTime;
        }
        rb.AddForce(force);

        // flip the Enemy sprite horizontally based on the player's position relative to the player's sprite position
        transform.localScale = new Vector3(Mathf.Sign((gameObject.transform.position - player.transform.position).x), 1, 1);

        //melee damage a.k.a. fish biting the player
        nearPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position) <= meleeDistance;
        if(nearPlayer && !bite_timer.runs()){
            player.GetComponent<BoxCollider2D>().SendMessage("addHealth", -getDamage(), SendMessageOptions.DontRequireReceiver);
            bite_timer.start(meleeCooldown);
        }
    }

    private static Vector3[] getNormalizedDirectionMap(){
        Vector3[] direction_map = new Vector3[]{new Vector3(0, 1, 0),
                                                new Vector3(.5f, .5f, 0),
                                                new Vector3(1, 0, 0),
                                                new Vector3(.5f, -.5f, 0),
                                                new Vector3(0, -1, 0),
                                                new Vector3(-.5f, -.5f, 0),
                                                new Vector3(-1, 0, 0),
                                                new Vector3(-.5f, .5f, 0)
                                                };
        foreach(Vector2 vec in direction_map){
            vec.Normalize();
        }
        return direction_map;
    }

    //negative hp damages, positive hp heals
    public void addHealth(int hp){
        health = Mathf.Clamp(health+hp,0,maxhealth);
        if(hp < 0){
            HS.Damage(-hp);
        }
        else{
            HS.Heal(hp);
        }
        
        if(health == 0){
            Debug.Log("You killed a "+_name+"!");
            Destroy(gameObject);
        }
    }

    public Rigidbody2D getRigidbody(){
        return rb;
    }

    public BoxCollider2D getCollider(){
        return bc;
    }

    public SpriteRenderer getSpriteRenderer(){
        return sr;
    }

    public float getSpeed(){
        return speed;
    }
    public float getDamage(){
        return damage;
    }

    public GameObject getPlayer(){
        return player;
    }

    public bool getNearPlayer(){
        return nearPlayer;
    }
}
