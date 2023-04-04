using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PufferFishData", menuName = "Enemy Data/Create PufferFishData")]
public class PufferFishData : ScriptableObject
{
    public string texturePath = "Sprites/PUFFERFISH_Single";
    public float textureScale = .85f;
    public float health = 100;
    public float damage = 25;
    public float meleeDamage = 25;
    public float chaseSpeed = 100000;
    public string gameObjectName = "Puffer fish emeny";
    public float meleeCooldown = .5f;
    public float stoppingDistance = 0.0f;
    public string path_to_controller = "Animation/Enemies/AC_Pufferfish";
   
}