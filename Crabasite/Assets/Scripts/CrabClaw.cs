using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClaw : MonoBehaviour
{
    [SerializeField] private float PullSpeed = 4;
    [SerializeField] private float PushSpeed = 4;
    
    void Update()
    {
        Vector3 mousePos_relative_to_player = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        mousePos_relative_to_player = new Vector3(mousePos_relative_to_player.x, mousePos_relative_to_player.y, 0);//force z=0 bc its somehow -10?!
        mousePos_relative_to_player.Normalize();

        if(Physics.Raycast(transform.position, mousePos_relative_to_player, out RaycastHit hit, Mathf.Infinity)){
            Vector3 playerPos_relative_to_hit = transform.position-hit.transform.position;
            playerPos_relative_to_hit.Normalize();

            if(Input.GetMouseButton(0)){
                hit.transform.position += playerPos_relative_to_hit*PullSpeed*Time.deltaTime;
            }
            if(Input.GetMouseButton(1)){
                hit.transform.position -= playerPos_relative_to_hit*PushSpeed*Time.deltaTime;
            }
        }
    }
}
