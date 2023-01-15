using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0f, 5000f)]
    public float speed = 2000f;



    
    private Rigidbody2D rb; 
    private Vector2 movement;
    private bool isDashing;
    public float dashDistance = 15f;
    public float dashDuration = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) StartCoroutine(Dash(movement));
    }

    private void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if(!isDashing) movePlayer(movement);
    }

    void movePlayer(Vector2 dir)
    {
        rb.AddForce(dir * speed);
    }

    IEnumerator Dash (Vector2 dir)
    {
        isDashing = true;
        rb.velocity = rb.velocity;
        rb.AddForce(dir * dashDistance, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
}
