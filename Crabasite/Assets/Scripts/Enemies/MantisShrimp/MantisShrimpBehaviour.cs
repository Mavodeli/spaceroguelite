using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisShrimpBehaviour : Enemy
{
    private MantisShrimpData msd;
    private Sprite shrimpWithSpear;
    private Sprite shrimpWithoutSpear;
    private Sprite shrimpSpear;
    private TimerObject spear1_timer;
    private TimerObject spear2_timer;

    void Awake()//use this instead of Start(), bc Enemy.cs already uses Start()!
    {
        msd = Resources.Load<MantisShrimpData>("ScriptableObjects/EnemyData/MantisShrimpData");
        shrimpWithSpear = getSprite(Resources.Load<Texture2D>(msd.texturePath));
        shrimpWithoutSpear = getSprite(Resources.Load<Texture2D>(msd.texturePathNoSpear));
        Texture2D spearTex = Resources.Load<Texture2D>(msd.texturePathSpear);
        shrimpSpear = getSprite(spearTex, new Rect(0, 0, spearTex.width, spearTex.height));
        initialSetup(msd.health,//health 
                        msd.health,//max health
                        msd.meleeDamage,//melee 
                        msd.meleeCooldown,//melee cooldown
                        msd.chaseSpeed,//speed
                        msd.gameObjectName,//name 
                        shrimpWithSpear,//sprite 
                        msd.textureScale,//sprite scale modifier
                        msd.stoppingDistance,//stopping distance
                        msd.path_to_controller
                        );
        spear1_timer = new TimerObject(msd.gameObjectName+" spear1_timer");
        spear2_timer = new TimerObject(msd.gameObjectName+" spear2_timer");
    }

    void LateUpdate()//bc Enemy.cs already uses Update()!
    {   
        //first spear
        if((Vector3.Distance(gameObject.transform.position, player.transform.position) <= msd.spearTriggerDistance) && !spear1_timer.runs()){
            getSpear();
            spear1_timer.start(msd.spearCooldown);
        }
        //second spear
        if((spear1_timer.getElapsedTime() >= msd.secondSpearDelay) && !spear2_timer.runs()){
            getSpear();
            spear2_timer.start(msd.spearCooldown);
            updateSprite(shrimpWithoutSpear, msd.textureScaleNoSpear);//update sprite only on second spear fired
        }
        //regrow comlete
        if(spear1_timer.getElapsedTime() >= msd.spearCooldown/2)
            updateSprite(shrimpWithSpear, msd.textureScale);
    }

    private void updateSprite(Sprite newSprite, float scale){
        sr.sprite = newSprite;
        sr.size = newSprite.bounds.extents*2;
        sr.size *= scale;
        bc.size = sr.size;
    }

    private static Sprite getSprite(Texture2D tex, Rect subpart = new Rect()){
        if(subpart == new Rect()) subpart = new Rect(0.0f, 0.0f, tex.width, tex.height);
        return Sprite.Create(tex, //texture
                                subpart, //subpart of the texture to create the sprite from
                                new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                0, //amount by which the sprite mesh should be expanded outwards
                                SpriteMeshType.FullRect //mesh type
                                );
    }

    private void getSpear(){
        GameObject spear = new GameObject();
        EnemyProjectile script = spear.AddComponent<EnemyProjectile>();
        script.Setup(gameObject, player, shrimpSpear, msd.textureScaleSpear, msd.spearSpeed, delegate (Collider2D other){
            if ((other.gameObject.tag == "Player") || (other.gameObject.tag == "Enemy")){
                other.SendMessage("addHealth", -msd.damage, SendMessageOptions.DontRequireReceiver);
            }
        });
    }
}
