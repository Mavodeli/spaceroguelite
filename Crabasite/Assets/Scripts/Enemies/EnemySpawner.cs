using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int fishiesCount1 = 5;
    [SerializeField]
    private int fishiesCount2 = 5;

    enum Type
    {PufferFish,MantisShrimp,AnglerFish,MorayEel};

    [SerializeField]
    Type EnemyType1 = new Type();
    [SerializeField]
    Type EnemyType2 = new Type();

    void Start()
    {
        spawnFishies(EnemyType1, EnemyType2);
    }

    void spawnFishies(Type enemy1, Type enemy2)
    {
        for (int i = 0; i < fishiesCount1; i++)
        {
            GameObject fish = new GameObject();
            if (enemy1 == Type.PufferFish) fish.AddComponent<PufferFishBehaviour>();
            if (enemy1 == Type.MantisShrimp) fish.AddComponent<MantisShrimpBehaviour>();
            if (enemy1 == Type.AnglerFish) fish.AddComponent<AnglerFishBehaviour>();
            if (enemy1 == Type.MorayEel) fish.AddComponent<MorayEelBehaviour>();
            fish.transform.position = new Vector3(Mathf.Pow((-1), i) * i, 2 * Mathf.Pow((-1), i) * i, 0);
        }
        for (int i = 0; i < fishiesCount2; i++)
        {
            GameObject fish = new GameObject();
            if (enemy2 == Type.PufferFish) fish.AddComponent<PufferFishBehaviour>();
            if (enemy2 == Type.MantisShrimp) fish.AddComponent<MantisShrimpBehaviour>();
            if (enemy2 == Type.AnglerFish) fish.AddComponent<AnglerFishBehaviour>();
            if (enemy2 == Type.MorayEel) fish.AddComponent<MorayEelBehaviour>();
            fish.transform.position = new Vector3(Mathf.Pow((-1), i) * i+1, Mathf.Pow((-1), i) * i+1, 0);
        }
    }
}
