using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorayEelBehaviour : Enemy
{
    private MorayEelData med;
    private Sprite morayEelwithProjectile;
    private Sprite morayEelwithoutProjectile;
    private Sprite projectile;
    private TimerObject projectile_timer;

    void Awake()//use this instead of Start(), bc Enemy.cs already uses Start()!
    {
        med = Resources.Load<MorayEelData>("Scriptable Objects/EnemyData/MorayEelData");
        morayEelwithProjectile = getSprite(Resources.Load<Texture2D>(med.texturePath));
        morayEelwithoutProjectile = getSprite(Resources.Load<Texture2D>(med.texturePathNoProjectile));
        projectile = getSprite(Resources.Load<Texture2D>(med.texturePathProjectile));
        initialSetup(med.health,//health 
                        med.health,//max health
                        med.meleeDamage,//melee 
                        med.meleeCooldown,//melee cooldown
                        med.chaseSpeed,//speed
                        med.gameObjectName,//name 
                        morayEelwithProjectile,//sprite 
                        med.textureScale,//sprite scale modifier
                        med.stoppingDistance//stopping distance
                        );
        projectile_timer = new TimerObject();
    }

    void LateUpdate()//bc Enemy.cs already uses Update()!
    {
    }

    private void updateSprite(Sprite newSprite, float scale)
    {
        sr.sprite = newSprite;
        sr.size = newSprite.bounds.extents * 2;
        sr.size *= scale;
    }

    private static Sprite getSprite(Texture2D tex)
    {
        return Sprite.Create(tex, //texture
                                new Rect(0.0f, 0.0f, tex.width, tex.height), //subpart of the texture to create the sprite from
                                new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                0, //amount by which the sprite mesh should be expanded outwards
                                SpriteMeshType.FullRect //mesh type
                                );
    }
}
