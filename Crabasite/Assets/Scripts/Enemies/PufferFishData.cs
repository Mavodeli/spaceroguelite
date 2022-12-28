using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New PufferFishData", menuName = "Enemy Data/Create New PufferFishData")]
public class PufferFishData : ScriptableObject
{
    public string texturePath = "Sprites/EnemyPlaceholder_256x256";
    public float textureScale = .5f;
    public float health = 100;
    public float damage = 25;
    public float chaseSpeed = 0.7f;
    public string gameObjectName = "Puffer fish emeny";
    public float biteCooldown = .5f;
}