using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AnglerFishData", menuName = "Enemy Data/Create AnglerFishData")]
public class AnglerFishData : ScriptableObject
{
    public string texturePath = "Sprites/angler_sheet";
    public float textureScale = 1.7f;
    public float health = 100;
    public float damage = 25;
    public float meleeDamage = 25;
    public float chaseSpeed = 300;
    public float maxReachAttraction = 7f;
    public float maxReachDamage = 3f;
    public float AttractionForce = 10f;
    public string gameObjectName = "Angler fish emeny";
    public float meleeCooldown = .5f;
    public float stoppingDistance = 6.8f;
    public string path_to_controller = "Animation/Enemies/AC_Anglerfish";
}