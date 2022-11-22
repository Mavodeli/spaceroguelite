using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClaw : MonoBehaviour
{
    [SerializeField] private float PullSpeed = 4;
    [SerializeField] private float PushSpeed = 4;
    [SerializeField] private float StartDelayDuration = 0.15f;
    [SerializeField] private float EndDelayDuration = 0.3f;
    // [SerializeField] private float PullSafetyDistance = 2;
    //IMPORTANT: the movement interpolation doesn't work bc the MIs can't use 
    // the provided direction vector to detect movement changes (bc that vector is constant) :( 
    private MovementInterpolation PushMI;
    private MovementInterpolation PullMI;
    //the Layer in which the ray checks for intersections (default: 'Enemies')
    public LayerMask DetectionLayer;

    void Awake(){
        PushMI = new MovementInterpolation();
        PullMI = new MovementInterpolation();
        PushMI.Awake();
        PullMI.Awake();
    }

    void Start(){
        PushMI.Start(PushSpeed, StartDelayDuration, EndDelayDuration);
        PullMI.Start(PullSpeed, StartDelayDuration, EndDelayDuration);
    }
    
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
            //playerPos_relative_to_hit: vector that points [location where the ray hits a collider] -> Player
            Vector3 playerPos_relative_to_hit = transform.position-hit.transform.position;
            playerPos_relative_to_hit.Normalize();

            PushMI.Update(mousePos_relative_to_player);
            float pushspeed = PushMI.getFrameSpeed();
            Vector3 pushdirection = PushMI.getFrameDirection();
            PullMI.Update(playerPos_relative_to_hit);
            float pullspeed = PullMI.getFrameSpeed();
            Vector3 pulldirection = PullMI.getFrameDirection();

            // float dist = Vector3.Distance(transform.position, hit.transform.position);

            //update the position of the object hit by the ray
            // if(Input.GetMouseButton(0) && dist > PullSafetyDistance){
            if(Input.GetMouseButton(0)){
                hit.transform.position += pulldirection*pullspeed*Time.deltaTime;
                Debug.Log(pullspeed);
            }
            if(Input.GetMouseButton(1)){
                hit.transform.position += pushdirection*pushspeed*Time.deltaTime;
                Debug.Log(pushspeed);
            }
        }
    }
}
