using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float Speed = 8;
    [SerializeField] private float StartDelayDuration = 0.2f;
    // [SerializeField] private float StartDelayModifier = 0.5f;
    [SerializeField] private float EndDelayDuration = 0.15f;
    // [SerializeField] private float EndDelayModifier = 0.5f;

    private Dictionary<string, Vector3> dir_map = new Dictionary<string, Vector3>();  
    private bool isMoving;
    private Timer start_delay_timer;
    private Timer end_delay_timer;
    private Vector3 end_delay_direction;

    void Awake(){
        start_delay_timer = new Timer();
        end_delay_timer = new Timer();
    }
    void Start()
    {
        start_delay_timer.Start();
        end_delay_timer.Start();
        dir_map.Add("w", new Vector3(0, 1, 0));
        dir_map.Add("a", new Vector3(-1, 0, 0));
        dir_map.Add("s", new Vector3(0, -1, 0));
        dir_map.Add("d", new Vector3(1, 0, 0));
        isMoving = false;
    }

    void Update()
    {
        start_delay_timer.Update();
        end_delay_timer.Update();
        bool last_isMoving = isMoving;

        Vector3 direction = new Vector3(0, 0, 0);
        float speed = Speed;

        string[] mov_strings = new string[]{"w", "a", "s", "d"};
        for(int i = 0; i < mov_strings.GetLength(0); i++){
            if(Input.GetKey(mov_strings[i])){
                direction += dir_map[mov_strings[i]];
            }
        }
        isMoving = direction != new Vector3(0, 0, 0);
        if(isMoving){end_delay_direction = direction;}

        if(isMoving && !last_isMoving){// false -> true
            end_delay_timer.stop();
            end_delay_direction = new Vector3(0, 0, 0);
            start_delay_timer.start(StartDelayDuration);
        }
        if(!isMoving && last_isMoving){// true -> false
            start_delay_timer.stop();
            end_delay_timer.start(EndDelayDuration);
        }

        if(start_delay_timer.is_running()){
            speed = speed * (1-(StartDelayDuration-start_delay_timer.get_elapsed_time()));
        }
        if(end_delay_timer.is_running()){
            direction = end_delay_direction;
            speed = speed * (EndDelayDuration-end_delay_timer.get_elapsed_time());
        }

        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
    }
}
