using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClaw : MonoBehaviour
{
    [SerializeField] private float PullSpeed = 4;
    [SerializeField] private float PushSpeed = 4;
    //the Layer in which the ray checks for intersections (default: 'Enemies')
    public LayerMask DetectionLayer;
    
    void Update()
    {   
        //mousePos_relative_to_player: vector that points Player -> Mouse Cursor
        Vector3 mousePos_relative_to_player = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        mousePos_relative_to_player = new Vector3(mousePos_relative_to_player.x, mousePos_relative_to_player.y, 0);//force z=0 bc its somehow -10?!
        mousePos_relative_to_player.Normalize();

        //Ray characteristics
        //- origin: Player Position
        //- direction: normalized Mouse Cursor Position
        //- maximal t: +infinity
        if(Physics.Raycast(transform.position, mousePos_relative_to_player, out RaycastHit hit, Mathf.Infinity, DetectionLayer)){
            //playerPos_relative_to_hit: vector that points [location where the ray hit a collider] -> Player
            Vector3 playerPos_relative_to_hit = transform.position-hit.transform.position;
            playerPos_relative_to_hit.Normalize();

            //update the position of the object hit by the ray
            if(Input.GetMouseButton(0)){
                hit.transform.position += playerPos_relative_to_hit*PullSpeed*Time.deltaTime;
            }
            if(Input.GetMouseButton(1)){
                hit.transform.position -= playerPos_relative_to_hit*PushSpeed*Time.deltaTime;
            }
        }
    }
}
