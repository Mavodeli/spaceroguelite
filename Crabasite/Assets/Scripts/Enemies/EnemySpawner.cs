using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int fishiesCount = 5;
    [SerializeField]
    private string enemyType = "pf";

    void Start()
    {
        for(int i = 0; i < fishiesCount; i++){
            GameObject fish = new GameObject();
            if(enemyType == "pf") fish.AddComponent<PufferFishBehaviour>();
            if(enemyType == "ms") fish.AddComponent<MantisShrimpBehaviour>();
            if(enemyType == "af") fish.AddComponent<AnglerFishBehaviour>();
            fish.transform.position = new Vector3(Mathf.Pow((-1), i)*i, 2*Mathf.Pow((-1), i)*i, 0);
        }
    }

    void Update()
    {

    }
}
