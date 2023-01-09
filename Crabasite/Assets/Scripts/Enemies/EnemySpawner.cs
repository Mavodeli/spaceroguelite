using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int fishiesCount = 5;
    [SerializeField]
    //private string enemyType = "pf";

    enum Type
    {PufferFish,MantisShrimp,AnglerFish,MorayEel};

    [SerializeField]
    Type EnemyType = new Type();

    void Start()
    {
        switch (EnemyType)
        {
            case Type.PufferFish:
                spawnFishies("pf");
                break;
            case Type.MantisShrimp:
                spawnFishies("ms");
                break;
            case Type.AnglerFish:
                spawnFishies("af");
                break;
            case Type.MorayEel:
                spawnFishies("me");
                break;  

        }
    }

    void spawnFishies(string enemy)
    {
        for (int i = 0; i < fishiesCount; i++)
        {
            GameObject fish = new GameObject();
            if (enemy == "pf") fish.AddComponent<PufferFishBehaviour>();
            if (enemy == "ms") fish.AddComponent<MantisShrimpBehaviour>();
            if (enemy == "af") fish.AddComponent<AnglerFishBehaviour>();
            if (enemy == "me") fish.AddComponent<MorayEelBehaviour>();
            fish.transform.position = new Vector3(Mathf.Pow((-1), i) * i, 2 * Mathf.Pow((-1), i) * i, 0);
        }
    }
}
