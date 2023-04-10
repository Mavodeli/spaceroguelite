using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    //HealthSystem
    private HealthSystem HS;

    //Sound Controller
    protected GameObject soundController;

    //Gameplay Properties
    private float health;
    private float maxhealth;
    private float speed;
    private string _name;

    //Components
    private Rigidbody2D rb;
    private Seeker seeker;
    protected Animator animator;
    protected SpriteRenderer sr;
    protected BoxCollider2D bc;
    protected HealthBar hb;

    //melee damage
    private float meleeDamage;
    private float meleeDistance;
    private float meleeCooldown;
    private TimerObject bite_timer;
    protected bool nearPlayer;

    //Pathfinding
    private Path path;
    private int currentWaypoint;
    private float nextWaypointDistance;
    protected float stoppingDistance;
    protected float idleDistance;
    protected GameObject player;

    //Misc
    private TimerObject paralyze_timer;
    private string[] itemsToDrop;
    
	
    protected void initialSetup(float _health,
                                float _maxhealth,
                                float _meleeDamage,
                                float _meleeCooldown,
                                float _speed, 
                                string name,
                                Sprite _sprite, 
                                float spriteScale,
                                float _stoppingDistance,
                                string path_to_controller = null
                                )
    {
        //setup properties
        health = _health;
        maxhealth = _maxhealth;
        meleeDamage = _meleeDamage;
        meleeCooldown = _meleeCooldown;
        _name = name;
        speed = _speed;
        nearPlayer = false;

        // animator
        Animator animator = GetComponent<Animator>();
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(path_to_controller);
        animator.runtimeAnimatorController = controller;

        //setup name, tag & layer
        gameObject.name = _name;
        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Raycast");

        //setup SpriteRenderer
        // sr = gameObject.AddComponent<SpriteRenderer>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        // sr.drawMode = SpriteDrawMode.Sliced;//needed for scaling the sprite
        sr.size *= spriteScale;
        // sr.sortingOrder = 1;
        if(SceneManager.GetActiveScene().name == "Level 2 - abandoned spaceship")
            sr.color *= new Color(1, 1, 1, .7f);

        //setup HealthSystem
        HS = new HealthSystem((int) health, (int) maxhealth);
        hb = HS.attachHealthBar(gameObject, sr.size.y/2+.1f);

        //setup Rigidbody2D
        // rb = gameObject.AddComponent<Rigidbody2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        // rb.gravityScale = 0;
        // rb.drag = 1;
        // rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // rb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/EnemyMaterial");

        //setup BoxCollider2D
        // bc = gameObject.AddComponent<BoxCollider2D>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        bc.size = sr.size;

        //setup Pathfinding
        // seeker = gameObject.AddComponent<Seeker>();
        seeker = gameObject.GetComponent<Seeker>();
        currentWaypoint = 0;
        nextWaypointDistance = 3.0f;
        stoppingDistance = _stoppingDistance;
        idleDistance = 8;
        player = GameObject.FindGameObjectWithTag("Player");
        // gameObject.AddComponent<DynamicGridObstacle>();

        //setup melee damage
        bite_timer = new TimerObject(_name+" bite_timer");

        paralyze_timer = new TimerObject(_name+" paralyze_timer");

        itemsToDrop = new string[0];
    }

    private void Start(){
        soundController = GameObject.Find("Sounds");
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

    private void FixedUpdate()
    {
        //movement
        Vector3 force;
        float distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (distanceToPlayer >= stoppingDistance && distanceToPlayer < idleDistance)
        {
            if (path == null) return;
            if (currentWaypoint >= path.vectorPath.Count) return;
            Vector3 direction = path.vectorPath[currentWaypoint] - gameObject.transform.position;
            direction.Normalize();
            force = direction * speed;
            if (Vector2.Distance(gameObject.transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                currentWaypoint++;
        }
        else
        {
            Vector3[] map = getNormalizedDirectionMap();
            Vector3 direction = map[Random.Range(0, map.Length - 1)];
            force = direction * speed;
        }

        //paralyze?
        if (paralyze_timer.runs())
        {
            force *= 0.01f;
        }

        rb.AddForce(force);
    }

    // private void Update(){
    //     // flip the Enemy sprite horizontally based on the player's position relative to the player's sprite position
    //     sr.flipX = Mathf.Sign((gameObject.transform.position - player.transform.position).x) > 0;
    //     if(_name == "Puffer fish emeny") sr.flipX = !sr.flipX;

    //     //melee damage a.k.a. fish biting the player
    //     nearPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position) <= meleeDistance;

    //     RaycastHit2D selfHit = Physics2D.Raycast(gameObject.transform.position, (player.transform.position-gameObject.transform.position).normalized, Mathf.Infinity, LayerMask.GetMask("Raycast"));
    //     meleeDistance = selfHit.collider != null ? Vector3.Distance(gameObject.transform.position, new Vector3(selfHit.point.x, selfHit.point.y, 0)) : 0;
    //     // meleeDistance += .5f;
    //     RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, (player.transform.position-gameObject.transform.position).normalized, meleeDistance, LayerMask.GetMask("Player"));
    //     Debug.DrawRay(gameObject.transform.position, (player.transform.position-gameObject.transform.position).normalized*meleeDistance, Color.red);
    //     bool rayHitsPlayer = hit.collider != null ? hit.transform.gameObject.tag == "Player" : false;

    //     if(nearPlayer && rayHitsPlayer && !bite_timer.runs()){
    //         soundController.SendMessage("playSound", new SoundParameter("EnemySound_MeleeAttack", this.gameObject, 0.15f, false), SendMessageOptions.DontRequireReceiver);
    //         player.GetComponent<BoxCollider2D>().SendMessage("addHealth", -meleeDamage, SendMessageOptions.DontRequireReceiver);
    //         bite_timer.start(meleeCooldown);
    //     }
    // }

    private void Update(){
        // flip the Enemy sprite horizontally based on the player's position relative to the player's sprite position
        sr.flipX = Mathf.Sign((gameObject.transform.position - player.transform.position).x) > 0;
        if(_name == "Puffer fish emeny") sr.flipX = !sr.flipX;

        if(bc.IsTouching(player.GetComponent<Collider2D>()) && !bite_timer.runs()){
            soundController.SendMessage("playSound", new SoundParameter("EnemySound_MeleeAttack", this.gameObject, 0.15f, false), SendMessageOptions.DontRequireReceiver);
            player.GetComponent<BoxCollider2D>().SendMessage("addHealth", -meleeDamage, SendMessageOptions.DontRequireReceiver);
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
            soundController.SendMessage("playSound", new SoundParameter("EnemyDeath", this.gameObject, 1f, false), SendMessageOptions.DontRequireReceiver);
            
            foreach(string id in itemsToDrop){
                GameObject GH = GameObject.FindGameObjectWithTag("GameHandler");
                object[] args = new object[]{id, transform.position};
                GH.SendMessage("spawnItem", args, SendMessageOptions.DontRequireReceiver);
            }
            Destroy(gameObject);
        }
    }

    public void paralyze(float duration){
        if(!paralyze_timer.runs())
            paralyze_timer.start(duration);
    }

    public void setItemsToDrop(string[] IDs){
        itemsToDrop = IDs;
    }

    public void adjustHealthbarOffsetBy(float factor){
        hb.transform.localPosition *= factor;
    }
}
