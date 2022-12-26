using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFishBehaviour : Enemy
{
    public GameObject player;
    public float maxReachAttraction = 7f; // defines the Reach Value of the Attraction
    public float maxReachDamage = 3f; // defines the Range in which the enemy starts to deal damage
    public float AttractionForce = 50f; // defines the Force Value of the Attraction
    Transform enemyTransform;
    Rigidbody2D enemyRigidbody;
    
    public float chaseSpeed = 5f;



    void Start()
    {
        player = GameObject.Find("Player");

        enemyTransform = transform;
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        Texture2D _sprite = Resources.Load<Texture2D>("Sprites/AnglerFish_256x256_New3");
        Sprite sprite = Sprite.Create(_sprite, //texture
                                        new Rect(0.0f, 0.0f, _sprite.width, _sprite.height), //subpart of the texture to create the sprite from
                                        new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                        100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                        0, //amount by which the sprite mesh should be expanded outwards
                                        SpriteMeshType.FullRect //mesh type
                                        );
        initialSetup(100,//health 
                        100,//max health 
                        10,//damage
                        0.5f,//speed
                        "Angler fish enemy",//name 
                        sprite,//sprite 
                        0.5f//sprite scale modifier
                        );
    }


    // Update is called once per frame
    void Update()
    {
        /* makes the enemy move towards the player. */
        Vector3 chaseDirection = player.transform.position - enemyTransform.position;
        chaseDirection.Normalize();
        enemyRigidbody.MovePosition(enemyTransform.position + chaseDirection * chaseSpeed * Time.deltaTime);

        /* makes the enemy pull the player towards it. */
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < maxReachAttraction)
        {
        Vector2 pullDirection = (Vector2)(transform.position - player.transform.position);
        player.GetComponent<Rigidbody2D>().AddForce(pullDirection * AttractionForce);
        }
        if (distance < maxReachDamage)
        {
            // make player loose health depending on tiks with the defined damage of the enemy.
        }
    }
}

