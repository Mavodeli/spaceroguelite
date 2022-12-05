using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    //this is NOT a MonoBehaviour file!
    //that means whenever you use this timer, make sure to call its Start() and Update() in the corresponding functions of the caller file!
    //e.g. like it is done in Movement.cs
    private float wait_time;
    private float elapsed_time;
    private bool isRunning;

    public void start(float _wait_time){
        if(!isRunning){
            wait_time = _wait_time;
            elapsed_time = 0;
            isRunning = true;
        }
    }

    public bool is_running(){
        return isRunning;
    }

    public float get_elapsed_time(){
        return elapsed_time;
    }

    public void stop(){
        wait_time = 0;
        elapsed_time = 0;
        isRunning = false;
    }

    public void Start(){
        isRunning = false;
    }

    public void Update()
    {
        if(isRunning){
            if(elapsed_time >= wait_time){
                stop();
            }
            else{
                elapsed_time += Time.deltaTime;
            }
        }
    }
}
