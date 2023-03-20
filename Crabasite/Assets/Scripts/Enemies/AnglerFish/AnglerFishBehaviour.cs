using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFishBehaviour : Enemy
{
    public float maxReachAttraction = 7f; // defines the Reach Value of the Attraction
    public float maxReachDamage = 3f; // defines the Range in which the enemy starts to deal damage
    public float AttractionForce = 10f; // defines the Force Value of the Attraction
    private AnglerFishData afd;

    void Awake()//use this instead of Start(), bc Enemy.cs already uses Start()!
    {
        afd = Resources.Load<AnglerFishData>("ScriptableObjects/EnemyData/AnglerFishData");
        Texture2D _sprite = Resources.Load<Texture2D>(afd.texturePath);
        Sprite sprite = Sprite.Create(_sprite, //texture
                                        new Rect(0.0f, 0.0f, _sprite.width, _sprite.height), //subpart of the texture to create the sprite from
                                        new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                        100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                        0, //amount by which the sprite mesh should be expanded outwards
                                        SpriteMeshType.FullRect //mesh type
                                        );
        initialSetup(afd.health,//health 
                        afd.health,//max health
                        afd.meleeDamage,//melee damage
                        afd.meleeCooldown,//melee cooldown
                        afd.chaseSpeed,//speed
                        afd.gameObjectName,//name 
                        sprite,//sprite 
                        afd.textureScale,//sprite scale modifier
                        afd.stoppingDistance,//stopping distance
                        afd.path_to_controller// path to animator controller
                        );//Sprites/AnglerFish_256x256_New3
    }

    // Update is called once per frame
    void LateUpdate()//bc Enemy.cs already uses Update()!
    {
        /* makes the enemy pull the player towards it. */
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < maxReachAttraction && !nearPlayer)
        {
            Vector2 pullDirection = (Vector2)(transform.position - player.transform.position);
            player.GetComponent<Rigidbody2D>().AddForce(pullDirection * AttractionForce);
        }
    }
}

