using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerObject
{
    private GameObject go = new GameObject();

    //creates a new GameObject for the timer. does *not* start the timer!!!
    //if autoDestroy is set: deletes the GameObject automatically when the timer runs out
    public TimerObject(string name = "Timer", bool autoDestroy = false){
        if(GameObject.FindWithTag("Timers") == null){
            // Debug.Log("Beep");
            GameObject new_timers = new GameObject();
            new_timers.tag = "Timers";
            new_timers.name = "Timers";
        }
        GameObject timers = GameObject.FindWithTag("Timers");//gameobj that holds all runnning timers
        go.transform.parent = timers.transform;
        go.tag = "Timer";
        go.name = name;
        go.AddComponent<Timer2>();
        if(autoDestroy)
            go.GetComponent<Timer2>().autoDestroy = autoDestroy;
    }

    //deletes the GameObject that holds this timer
    public void Delete(){
        GameObject.Destroy(go);
    }

    //starts the timer
    public void start(float _wait_time){
        if(go != null){
            Timer2 timer = go.GetComponent<Timer2>(); 
            if(!timer.isRunning){
                timer.wait_time = _wait_time;
                timer.elapsed_time = 0;
                timer.isRunning = true;
            }
        }
    }

    //the timer stops automatically when it runs out, this function is for interrupting the timer
    public void stop(){
        if(go != null){
            Timer2 timer = go.GetComponent<Timer2>(); 
            timer.wait_time = 0;
            timer.elapsed_time = 0;
            timer.isRunning = false;
        }
    }

    //returns true if the timer is still running, false otherwise
    public bool runs(){
        if(go != null){
            Timer2 timer = go.GetComponent<Timer2>();
            return timer.isRunning;
        }
        return false;
    }

    public float getElapsedTime(){
        if(go != null){
            Timer2 timer = go.GetComponent<Timer2>();
            if(timer.isRunning) return timer.elapsed_time;
        }
        return 0;
    }
}

public class Timer2 : MonoBehaviour
{
    public float wait_time;
    public float elapsed_time;
    public bool isRunning = false;
    public bool autoDestroy = false;

    void Update()
    {
        if(isRunning){
            // Debug.Log("elapsed_time: "+elapsed_time);
            if(elapsed_time >= wait_time){//stop
                if(autoDestroy){
                    GameObject.Destroy(gameObject);
                    return;
                }
                wait_time = 0;
                elapsed_time = 0;
                isRunning = false;
            }
            else{//running
                elapsed_time += Time.deltaTime;
            }
        }
    }
}
