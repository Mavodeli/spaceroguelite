using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInterpolation
{
    private float StartDelayDuration;
    private float EndDelayDuration;
    private float Speed;

    //bool for checking if movement recently began or stopped
    private bool isMoving;
    private Timer start_delay_timer;
    private Timer end_delay_timer;
    //storage var for fade-out movement direction
    private Vector3 end_delay_direction;
    private Vector3 frameDirection;
    private float frameSpeed;

    public void Awake(){
        //create Timer instances
        start_delay_timer = new Timer();
        end_delay_timer = new Timer();
    }
    public void Start(float _Speed, float _StartDelayDuration, float _EndDelayDuration)
    {
        //init class attributes
        //call Start() for each timer as Timer is just a plain script and not a MonoBehaviour
        start_delay_timer.Start();
        end_delay_timer.Start();
        StartDelayDuration = _StartDelayDuration;
        EndDelayDuration = _EndDelayDuration;
        Speed = _Speed;
        isMoving = false;
    }

    public void Update(Vector3 direction)
    {
        //call Update() for each timer as Timer is just a plain script and not a MonoBehaviour
        start_delay_timer.Update();
        end_delay_timer.Update();
        //stored for comparison with the movement situation (isMoving) in this frame
        //(to detect if the player begins or stops pressing wasd in-between frames)
        bool last_isMoving = isMoving;
        float speed = Speed;
        isMoving = direction != new Vector3(0, 0, 0);
        frameDirection = direction;
        //set the fade-out movement direction to the last known direction vector
        if(isMoving){end_delay_direction = direction;}

        //checks if the player begins pressing wasd in-between frames
        if(isMoving && !last_isMoving){// false -> true (aka begin movement)
            end_delay_timer.stop();
            end_delay_direction = new Vector3(0, 0, 0);
            start_delay_timer.start(Mathf.Max(StartDelayDuration,0));
        }
        //checks if the player stops pressing wasd in-between frames
        else if(!isMoving && last_isMoving){// true -> false (aka stop movement)
            start_delay_timer.stop();
            end_delay_timer.start(Mathf.Max(EndDelayDuration,0));
        }
        else{
            //modify speed based on running timers (for fade-in/fade-out)
            if(start_delay_timer.is_running()){
                speed = speed * Mathf.Clamp(1-(StartDelayDuration-start_delay_timer.get_elapsed_time()),0,1);
            }
            if(end_delay_timer.is_running()){
                speed = speed * Mathf.Clamp(EndDelayDuration-end_delay_timer.get_elapsed_time(),0,1);
                frameDirection = end_delay_direction;
            }
        }
        frameSpeed = speed;
    }

    public float getFrameSpeed(){
        return frameSpeed;
    }

    public Vector3 getFrameDirection(){
        return frameDirection;
    }
}
