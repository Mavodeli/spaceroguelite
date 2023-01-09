using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MorayEelData", menuName = "Enemy Data/Create MorayEelData")]
public class MorayEelData : ScriptableObject
{
    public string texturePath = "Sprites/EnemyPlaceholder_MantisShrimp_249x171_Ammo";
    public float textureScale = 1;
    public float health = 70;
    public float damage = 10;
    public float meleeDamage = 20;
    public float chaseSpeed = 800;
    public float projectileSpeed = 300;
    public string gameObjectName = "Moray Eel emeny";
    public float projectileCooldown = 5;
    public float meleeCooldown = .5f;
    public float stoppingDistance = 10;
    public string texturePathNoProjectile = "Sprites/EnemyPlaceholder_MantisShrimp_249x171_noAmmo";
    public string texturePathProjectile = "Sprites/EnemyPlaceholder_MantisShrimp_55x12_Projectile";
    public float textureScaleProjectile = 1.5f;
}