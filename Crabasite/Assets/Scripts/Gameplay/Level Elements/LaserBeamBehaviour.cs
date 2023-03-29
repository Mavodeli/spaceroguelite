using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamBehaviour : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private float max_beam_length;
    private Vector2 emitter_position;
    private TimerObject laser_uptime_timer;
    [SerializeField] private float laser_uptime = 0;
    private TimerObject laser_downtime_timer;
    [SerializeField] private float laser_downtime = 0;
    private TimerObject laser_offset_timer;
    [SerializeField] private float laser_offset_downtime = 0;
    
    void Start(){
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        sr.size *= new Vector2(sr.transform.localScale.x, sr.transform.localScale.y);
        bc = GetComponent<BoxCollider2D>();
        bc.size = sr.size;
        max_beam_length = sr.size.y;
        emitter_position = new Vector2(transform.position.x, transform.position.y+sr.size.y/2);

        laser_uptime_timer = new TimerObject(gameObject.name+" laser_uptime_timer");
        laser_uptime_timer.setOnRunningOut(delegate(){laser_downtime_timer.start(laser_downtime);});

        laser_downtime_timer = new TimerObject(gameObject.name+" laser_downtime_timer");
        laser_downtime_timer.setOnRunningOut(delegate(){laser_uptime_timer.start(laser_uptime);});

        laser_offset_timer = new TimerObject(gameObject.name+" laser_offset_timer", true);
        laser_offset_timer.setOnRunningOut(delegate(){laser_uptime_timer.start(laser_uptime);});
        laser_offset_timer.start(laser_offset_downtime);
    }

    void Update()
    {
        float current_beam_length = max_beam_length;

        float to_uptime_duration = laser_uptime/10;
        float to_downtime_duration = laser_downtime/10;

        if(laser_uptime_timer.runs()){
            float elapsed_uptime = laser_uptime_timer.getElapsedTime();

            if(floatIsBetweenInclusive(elapsed_uptime, 0, to_uptime_duration))
                current_beam_length = max_beam_length*(elapsed_uptime/to_uptime_duration);
            else
                current_beam_length = max_beam_length;
        }

        if(laser_downtime_timer.runs()){
            float elapsed_downtime = laser_downtime_timer.getElapsedTime();

            if(floatIsBetweenInclusive(elapsed_downtime, 0, to_downtime_duration))
                current_beam_length = max_beam_length*(1-(elapsed_downtime/to_downtime_duration));
            else
                current_beam_length = 0;
        }

        if(laser_offset_timer.runs())
            current_beam_length = 0;

        RaycastHit2D hit = Physics2D.Raycast(emitter_position, new Vector2(0, -1), current_beam_length, LayerMask.GetMask("Raycast"));
        if(hit.collider != null){
            current_beam_length = Mathf.Min(current_beam_length, emitter_position.y-hit.point.y);
        }

        drawBeamWithLength(current_beam_length);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"){
            other.gameObject.SendMessage("addHealth", -90, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void drawBeamWithLength(float length){
        if(float.IsNaN(length)) return;
        float beam_center_modifier = (emitter_position.y-(length/2))-transform.position.y;

        //modify box collider
        bc.offset = new Vector2(0, beam_center_modifier);
        bc.size = new Vector2(bc.size.x, length);

        //modify sprite container
        GameObject child = transform.GetChild(0).gameObject;
        child.transform.localScale = new Vector3(1, length/max_beam_length, 0);
        child.transform.localPosition = new Vector3(0, beam_center_modifier, 0);
    }

    private bool floatIsBetweenInclusive(float value, float lower_bound, float upper_bound){
        return (value >= lower_bound) && (value <= upper_bound);
    }
}
