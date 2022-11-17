using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float Speed = 8;
    [SerializeField] private float StartDelayDuration = 0.2f;
    [SerializeField] private float EndDelayDuration = 0.15f;
    [SerializeField] private float DashCooldown = 1;
    [SerializeField] private float DashDuration = 0.1f;
    [SerializeField] private float DashModifier = 4;

    //dict that maps input keys to the corresponding direction vectors
    private Dictionary<string, Vector3> dir_map = new Dictionary<string, Vector3>();

    //bool for checking if movement recently began or stopped
    private bool isMoving;
    private Timer start_delay_timer;
    private Timer end_delay_timer;
    //storage var for fade-out movement direction
    private Vector3 end_delay_direction;

    //dash
    private Timer dash_cooldown;
    private Timer dash_active_frames;

    void Awake(){
        //create Timer instances
        start_delay_timer = new Timer();
        end_delay_timer = new Timer();
        dash_cooldown = new Timer();
        dash_active_frames = new Timer();
    }
    void Start()
    {
        //init class attributes
        //call Start() for each timer as Timer is just a plain script and not a MonoBehaviour
        start_delay_timer.Start();
        end_delay_timer.Start();
        dash_active_frames.Start();
        dash_cooldown.Start();
        dir_map.Add("w", new Vector3(0, 1, 0));
        dir_map.Add("a", new Vector3(-1, 0, 0));
        dir_map.Add("s", new Vector3(0, -1, 0));
        dir_map.Add("d", new Vector3(1, 0, 0));
        isMoving = false;
    }

    void Update()
    {
        //call Update() for each timer as Timer is just a plain script and not a MonoBehaviour
        start_delay_timer.Update();
        end_delay_timer.Update();
        dash_active_frames.Update();
        dash_cooldown.Update();
        //stored for comparison with the movement situation (isMoving) in this frame
        //(to detect if the player begins or stops pressing wasd in-between frames)
        bool last_isMoving = isMoving;

        //reset direction from last frame
        Vector3 direction = new Vector3(0, 0, 0);
        //get speed from the SerializeField
        float speed = Speed;

        //check the map for all required movement directions
        //the loop can apply multiple direction vectors(!) e.g. for diagonal movement
        string[] mov_strings = new string[]{"w", "a", "s", "d"};
        for(int i = 0; i < mov_strings.GetLength(0); i++){
            if(Input.GetKey(mov_strings[i])){
                direction += dir_map[mov_strings[i]];
            }
        }
        isMoving = direction != new Vector3(0, 0, 0);
        //set the fade-out movement direction to the last known direction vector
        if(isMoving){end_delay_direction = direction;}

        //checks if the player begins pressing wasd in-between frames
        if(isMoving && !last_isMoving){// false -> true (aka begin movement)
            end_delay_timer.stop();
            end_delay_direction = new Vector3(0, 0, 0);
            start_delay_timer.start(StartDelayDuration);
        }
        //checks if the player stops pressing wasd in-between frames
        if(!isMoving && last_isMoving){// true -> false (aka stop movement)
            start_delay_timer.stop();
            end_delay_timer.start(EndDelayDuration);
        }

        //modify speed based on running timers (for fade-in/fade-out)
        if(start_delay_timer.is_running()){
            speed = speed * (1-(StartDelayDuration-start_delay_timer.get_elapsed_time()));
        }
        if(end_delay_timer.is_running()){
            direction = end_delay_direction;
            speed = speed * (EndDelayDuration-end_delay_timer.get_elapsed_time());
        }

        //dash
        if(Input.GetKey("left shift") && !dash_cooldown.is_running()){
            dash_active_frames.start(DashDuration);
            dash_cooldown.start(DashCooldown);
        }
        if(dash_active_frames.is_running()){
            speed = speed*DashModifier;
        }

        //update gameObject (player) position
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
    }
}
