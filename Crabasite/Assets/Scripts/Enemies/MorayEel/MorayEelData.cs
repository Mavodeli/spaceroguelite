using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MorayEelData", menuName = "Enemy Data/Create MorayEelData")]
public class MorayEelData : ScriptableObject
{
    public string texturePath = "Sprites/onlyeel";
    public float textureScale = 1;
    public float health = 70;
    public float damage = 3;
    public float meleeDamage = 10;
    public float chaseSpeed = 1100;
    public float projectileSpeed = 700;
    public float projectileTriggerDistance = 12;
    public string gameObjectName = "Moray Eel emeny";
    public float projectileCooldown = 2;
    public float meleeCooldown = .3f;
    public float paralyzeDuration = 1.2f;
    public float stoppingDistance = 2;
    public string texturePathProjectile = "Sprites/onlyball";
    public float textureScaleProjectile = 1;
    public string path_to_controller = "Animation/Enemies/AC_Morey_Eel";
}