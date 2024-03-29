using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorayEelBehaviour : Enemy
{
    private MorayEelData med;
    private Sprite sprite;
    private Sprite projectile;
    private TimerObject projectile_timer;
    private TimerObject paralyze_timer;

    void Awake()//use this instead of Start(), bc Enemy.cs already uses Start()!
    {
        med = Resources.Load<MorayEelData>("ScriptableObjects/EnemyData/MorayEelData");
        sprite = getSprite(Resources.Load<Texture2D>(med.texturePath));
        projectile = getSprite(Resources.Load<Texture2D>(med.texturePathProjectile));
        initialSetup(med.health,//health 
                        med.health,//max health
                        med.meleeDamage,//melee 
                        med.meleeCooldown,//melee cooldown
                        med.chaseSpeed,//speed
                        med.gameObjectName,//name 
                        sprite,//sprite 
                        med.textureScale,//sprite scale modifier
                        med.stoppingDistance,//stopping distance
                        med.path_to_controller
                        );
        projectile_timer = new TimerObject(med.gameObjectName+" projectile_timer");
        adjustHealthbarOffsetBy(.6f);
    }

    void LateUpdate()//bc Enemy.cs already uses Update()!
    {
        if((Vector3.Distance(gameObject.transform.position, player.transform.position) <= med.projectileTriggerDistance) && !projectile_timer.runs()){
            GameObject obj = new GameObject();
            EnemyProjectile script = obj.AddComponent<EnemyProjectile>();
            script.Setup(gameObject, player, projectile, med.textureScaleProjectile, med.projectileSpeed, delegate (Collider2D other){
                if ((other.gameObject.tag == "Player") || (other.gameObject.tag == "Enemy")){
                    other.SendMessage("addHealth", -med.damage, SendMessageOptions.DontRequireReceiver);
                    other.SendMessage("paralyze", med.paralyzeDuration, SendMessageOptions.DontRequireReceiver);
                }
            });
            projectile_timer.start(med.projectileCooldown);
        }
    }

    private static Sprite getSprite(Texture2D tex)
    {
        return Sprite.Create(tex, //texture
                                new Rect(0.0f, 0.0f, tex.width/2, tex.height), //subpart of the texture to create the sprite from
                                new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                0, //amount by which the sprite mesh should be expanded outwards
                                SpriteMeshType.FullRect //mesh type
                                );
    }
}
