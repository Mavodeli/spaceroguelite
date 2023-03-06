using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalEntity : MonoBehaviour
{
    //Components
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected BoxCollider2D bc;

    //Delegates
    public delegate void OnPickup();

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }
}
