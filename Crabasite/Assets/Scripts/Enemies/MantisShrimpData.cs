using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MantisShrimpData", menuName = "Enemy Data/Create MantisShrimpData")]
public class MantisShrimpData : ScriptableObject
{
    public string texturePath = "Sprites/EnemyPlaceholder_MantisShrimp_256x256";
    public float textureScale = 1;
    public float health = 180;
    public float damage = 50;
    public float chaseSpeed = 0.5f;
    public string gameObjectName = "Mantis shrimp emeny";
    public float spearCooldown = 2.5f;
    public float stoppingDistance = 5.0f;
}