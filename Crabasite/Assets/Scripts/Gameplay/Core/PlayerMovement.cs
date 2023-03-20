using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0f, 5000f)]
    public float standard_speed = 2000f;

    private float current_speed = 2000f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 movement;
    private bool isDashing;
    public float dashDistance = 15f;
    public float dashDuration = 0.1f;
    public float dash_cooldown = 1f;

    private TimerObject paralyze_timer;
    private TimerObject dash_cooldown_timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        paralyze_timer = new TimerObject("Player paralyze_timer");
        dash_cooldown_timer = new TimerObject("Player dash_cooldown_timer");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !dash_cooldown_timer.runs()) StartCoroutine(Dash(movement));
        if (Input.GetKeyDown(KeyCode.A)) sr.flipX = true;
        if (Input.GetKeyDown(KeyCode.D)) sr.flipX = false;

    }

    private void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if(!isDashing) movePlayer(movement);
    }

    void movePlayer(Vector2 dir)
    {
        current_speed = standard_speed;

        if (paralyze_timer.runs())
        {
            current_speed *= 0.1f;
        }

        Vector2 force = dir * current_speed;
        if(rb.gravityScale != 0) 
            force.y = 0;

        rb.AddForce(force);
    }

    IEnumerator Dash (Vector2 dir)
    {
        isDashing = true;
        rb.velocity = rb.velocity;
        rb.AddForce(dir * dashDistance, ForceMode2D.Impulse);
        GameObject.Find("Sounds").SendMessage("playSound", new SoundParameter("PlayerDashSound", GameObject.Find("Player"), 1f, false));
        yield return new WaitForSeconds(dashDuration);
        dash_cooldown_timer.start(dash_cooldown);
        isDashing = false;
    }

    public void paralyze(float duration)
    {
        if (!paralyze_timer.runs())
            paralyze_timer.start(duration);
    }

    public Vector2 getMovement(){
        return movement;
    }
}
