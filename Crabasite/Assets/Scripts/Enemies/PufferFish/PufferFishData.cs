using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PufferFishData", menuName = "Enemy Data/Create PufferFishData")]
public class PufferFishData : ScriptableObject
{
    public string texturePath = "Sprites/EnemyPlaceholder_256x256";
    public float textureScale = .38f;
    public float health = 100;
    public float damage = 25;
    public float meleeDamage = 25;
    public float chaseSpeed = 400;
    public string gameObjectName = "Puffer fish emeny";
    public float meleeCooldown = .5f;
    public float stoppingDistance = 0.0f;
}