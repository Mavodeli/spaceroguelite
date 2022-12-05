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
    private Dictionary<string, Vector2> dir_map = new Dictionary<string, Vector2>();

    private MovementInterpolation MI;

    //dash
    private Timer dash_cooldown;
    private Timer dash_active_frames;

    void Awake(){
        MI = new MovementInterpolation();
        MI.Awake();
        //create Timer instances
        dash_cooldown = new Timer();
        dash_active_frames = new Timer();
    }
    void Start()
    {
        //init class attributes
        MI.Start(Speed, StartDelayDuration, EndDelayDuration);
        //call Start() for each timer as Timer is just a plain script and not a MonoBehaviour
        dash_active_frames.Start();
        dash_cooldown.Start();
        dir_map.Add("w", new Vector2(0, 1));
        dir_map.Add("a", new Vector2(-1, 0));
        dir_map.Add("s", new Vector2(0, -1));
        dir_map.Add("d", new Vector2(1, 0));
    }

    void Update()
    {
        //call Update() for each timer as Timer is just a plain script and not a MonoBehaviour
        dash_active_frames.Update();
        dash_cooldown.Update();

        //reset direction from last frame
        Vector2 direction = new Vector2(0, 0);

        //check the map for all required movement directions
        //the loop can apply multiple direction vectors(!) e.g. for diagonal movement
        string[] mov_strings = new string[]{"w", "a", "s", "d"};
        for(int i = 0; i < mov_strings.GetLength(0); i++){
            if(Input.GetKey(mov_strings[i])){
                direction += dir_map[mov_strings[i]];
            }
        }

        MI.Update((direction != new Vector2(0, 0)), direction);
        float speed = MI.getFrameSpeed();

        //dash
        if(Input.GetKey("left shift") && !dash_cooldown.is_running()){
            dash_active_frames.start(DashDuration);
            dash_cooldown.start(DashCooldown);
        }
        if(dash_active_frames.is_running()){
            speed = speed*DashModifier;
        }

        //update gameObject (player) position
        direction = MI.getFrameDirection();
        direction.Normalize();
        Vector3 dir = new Vector3(direction.x, direction.y, 0);
        transform.position += dir * speed * Time.deltaTime;
    }
}
