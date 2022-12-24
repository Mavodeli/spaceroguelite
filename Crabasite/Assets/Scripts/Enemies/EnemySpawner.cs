using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int fishiesCount = 5;

    void Start()
    {
        for(int i = 0; i < fishiesCount; i++){
            GameObject fish = new GameObject();
            fish.AddComponent<PufferFishBehaviour>();
            fish.transform.position = new Vector3(Mathf.Pow((-1), i)*i, 2*Mathf.Pow((-1), i)*i, 0);
        }
    }

    void Update()
    {
        
    }
}
