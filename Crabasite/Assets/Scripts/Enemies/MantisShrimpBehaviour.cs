using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisShrimpBehaviour : Enemy
{
    private MantisShrimpData msd;
    void Awake()//use this instead of Start(), bc Enemy.cs already uses Start()!
    {
        msd = Resources.Load<MantisShrimpData>("Scriptable Objects/EnemyData/MantisShrimpData");
        Texture2D _sprite = Resources.Load<Texture2D>(msd.texturePath);
        Sprite sprite = Sprite.Create(_sprite, //texture
                                        new Rect(0.0f, 0.0f, _sprite.width, _sprite.height), //subpart of the texture to create the sprite from
                                        new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                        100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                        0, //amount by which the sprite mesh should be expanded outwards
                                        SpriteMeshType.FullRect //mesh type
                                        );
        initialSetup(msd.health,//health 
                        msd.health,//max health 
                        msd.damage,//damage
                        msd.meleeDamage,//melee 
                        msd.meleeCooldown,//melee cooldown
                        msd.chaseSpeed,//speed
                        msd.gameObjectName,//name 
                        sprite,//sprite 
                        msd.textureScale,//sprite scale modifier
                        msd.stoppingDistance//stopping distance
                        );
    }

    void LateUpdate()//bc Enemy.cs already uses Update()!
    {
        //TODO
    }
}
