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
        msd = Resources.Load<MantisShrimpData>("Scriptable Objects/EnemyData/MantisShrimpData");
        shrimpWithSpear = getSprite(Resources.Load<Texture2D>(msd.texturePath));
        shrimpWithoutSpear = getSprite(Resources.Load<Texture2D>(msd.texturePathNoSpear));
        shrimpSpear = getSprite(Resources.Load<Texture2D>(msd.texturePathSpear));
        initialSetup(msd.health,//health 
                        msd.health,//max health
                        msd.meleeDamage,//melee 
                        msd.meleeCooldown,//melee cooldown
                        msd.chaseSpeed,//speed
                        msd.gameObjectName,//name 
                        shrimpWithSpear,//sprite 
                        msd.textureScale,//sprite scale modifier
                        msd.stoppingDistance//stopping distance
                        );
        spear1_timer = new TimerObject();
        spear2_timer = new TimerObject();
    }

    void LateUpdate()//bc Enemy.cs already uses Update()!
<<<<<<< Updated upstream
    {   
        //first spear
        if((Vector3.Distance(gameObject.transform.position, getPlayer().transform.position) <= msd.spearTriggerDistance) && !spear1_timer.runs()){
            GameObject spear = new GameObject();
            MantisShrimpSpear script = spear.AddComponent<MantisShrimpSpear>();
            script.Setup(gameObject, GameObject.FindGameObjectWithTag("Player"), shrimpSpear);
            spear1_timer.start(msd.spearCooldown);
            // updateSprite(shrimpWithoutSpear, msd.textureScaleNoSpear);
        }
        //second spear
        if((spear1_timer.getElapsedTime() >= msd.secondSpearDelay) && !spear2_timer.runs()){
=======
    {
        if((Vector3.Distance(gameObject.transform.position, player.transform.position) <= msd.spearTriggerDistance) && !spear_timer.runs()){
>>>>>>> Stashed changes
            GameObject spear = new GameObject();
            MantisShrimpSpear script = spear.AddComponent<MantisShrimpSpear>();
            script.Setup(gameObject, GameObject.FindGameObjectWithTag("Player"), shrimpSpear);
            spear2_timer.start(msd.spearCooldown);
            updateSprite(shrimpWithoutSpear, msd.textureScaleNoSpear);
        }
        //regrow comlete
        if(spear1_timer.getElapsedTime() >= msd.spearCooldown/2)
            updateSprite(shrimpWithSpear, msd.textureScale);
    }

    private void updateSprite(Sprite newSprite, float scale){
        sr.sprite = newSprite;
        sr.size = newSprite.bounds.extents*2;
        sr.size *= scale;
    }

    private static Sprite getSprite(Texture2D tex){
        return Sprite.Create(tex, //texture
                                new Rect(0.0f, 0.0f, tex.width, tex.height), //subpart of the texture to create the sprite from
                                new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                0, //amount by which the sprite mesh should be expanded outwards
                                SpriteMeshType.FullRect //mesh type
                                );
    }
}
