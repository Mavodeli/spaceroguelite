using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomVelocityOnActivate : MonoBehaviour
{
    public float forceMultiplier = 1f;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnEnable()
    {
        var randomDirection = new Vector2(random1D(), random1D());

        var randomForce = forceMultiplier * Random.Range(0f, 5f);

        gameObject.GetComponent<Rigidbody2D>().AddForce(randomDirection * randomForce);
    }

    private float random1D()
    {
        return Random.Range(-1f, 1f);
    }
}
