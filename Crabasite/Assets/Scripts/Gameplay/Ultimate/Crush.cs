using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour
{
    public KeyCode key;
    public LayerMask layermask;
    public int radius;
    [SerializeField] Transform player;


    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            Use();
        } 
    }

    public void Use()
    {
        Vector3 center = player.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layermask);
        
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.SendMessage("EnemyTakeDmg", 5);
            Debug.Log(hitCollider);
        }

    }

}
