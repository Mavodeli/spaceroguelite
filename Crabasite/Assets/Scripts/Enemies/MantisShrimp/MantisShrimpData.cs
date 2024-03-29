using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MantisShrimpData", menuName = "Enemy Data/Create MantisShrimpData")]
public class MantisShrimpData : ScriptableObject
{
    public string texturePath = "Sprites/mantis_shrimp";
    public float textureScale = 1;
    public float health = 180;
    public float damage = 50;
    public float meleeDamage = 20;
    public float chaseSpeed = 400;
    public float spearSpeed = 300;
    public string gameObjectName = "Mantis shrimp emeny";
    public float spearCooldown = 5;
    public float secondSpearDelay = .3f;
    public float meleeCooldown = .5f;
    public float stoppingDistance = 10;
    public float spearTriggerDistance = 20;
    public string texturePathNoSpear = "Sprites/EnemyPlaceholder_MantisShrimp_249x171_noAmmo";
    public float textureScaleNoSpear = 1;
    public string texturePathSpear = "Sprites/projectile_mantis";
    public float textureScaleSpear = 1.5f;
    public string path_to_controller = "Animation/Enemies/AC_mantis_shrimp";
}