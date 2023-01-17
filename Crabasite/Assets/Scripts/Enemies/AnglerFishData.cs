using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AnglerFishData", menuName = "Enemy Data/Create AnglerFishData")]
public class AnglerFishData : ScriptableObject
{
    public string texturePath = "Sprites/AnglerFish_256x256_New3";
    public float textureScale = .38f;
    public float health = 100;
    public float damage = 25;
    public float meleeDamage = 25;
    public float chaseSpeed = 400;
    public string gameObjectName = "Angler fish emeny";
    public float meleeCooldown = .5f;
    public float stoppingDistance = 0.0f;
}