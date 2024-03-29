using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferFishBehaviour : Enemy
{
    private PufferFishData pfd;

    void Awake()//use this instead of Start(), bc Enemy.cs already uses Start()!
    {
        pfd = Resources.Load<PufferFishData>("ScriptableObjects/EnemyData/PufferFishData");
        Texture2D _sprite = Resources.Load<Texture2D>(pfd.texturePath);
        Sprite sprite = Sprite.Create(_sprite, //texture
                                        new Rect(0.0f, 0.0f, _sprite.width, _sprite.height), //subpart of the texture to create the sprite from
                                        new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                        100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                        0, //amount by which the sprite mesh should be expanded outwards
                                        SpriteMeshType.FullRect //mesh type
                                        );
        initialSetup(pfd.health,//health 
                        pfd.health,//max health
                        pfd.meleeDamage,//melee damage
                        pfd.meleeCooldown,//melee cooldown
                        pfd.chaseSpeed,//speed
                        pfd.gameObjectName,//name 
                        sprite,//sprite 
                        pfd.textureScale,//sprite scale modifier
                        pfd.stoppingDistance,//stopping distance
                        pfd.path_to_controller// path to animator controller
                        );

    }

    void LateUpdate()//bc Enemy.cs already uses Update()!
    {
        float distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
        
        if(Random.Range(-1f, 140f * 7f) < 0 && distanceToPlayer >= stoppingDistance && distanceToPlayer < idleDistance){ // with approximatly 140 updates per second a sound will be emitted approximatly every 7 seconds per enemy
            soundController.SendMessage("playSound", new SoundParameter("EnemySound_1", this.gameObject, 1f, false), SendMessageOptions.DontRequireReceiver);
        }
    }
}
