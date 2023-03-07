using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbody2D needs to be on self and not on children to detect collisions!
// (The last state can have Rigidbody2Ds but won't trigger this scripts detection then)
public class break_asteroid : MonoBehaviour
{
    // settings
    public GameObject player;
    public int requiredForce;
    public float breakCooldown = 3f;
    private Timer breakTimer;

    // state bounds
    private int breakState = 0;
    private int splitState = 3; // The state at which the split should occur (also maximum breakState)

    // the state GameObjects
    public GameObject state0;
    public GameObject state1;
    public GameObject state2;
    public GameObject state3;
    private GameObject[] stateMap; // is filled on Start()
    private bool collided = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (
            breakState < splitState
            & !breakTimer.is_running()
            & collision.gameObject != player
            & collision.relativeVelocity.magnitude > requiredForce
        )
        {
            breakState += 1;
            breakTimer.start(breakCooldown);
            collided = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        breakTimer = new Timer();

        stateMap = new GameObject[] { state0, state1, state2, state3 };
    }

    // Update is called once per frame
    void Update()
    {
        // update timer
        if (breakTimer != null)
        {
            breakTimer.Update();
        }

        // update active object if necessary
        if (collided)
        {
            for (int i = 0; i <= splitState; i++)
            {
                if (i == breakState)
                {
                    stateMap[i].SetActive(true);
                }
                else
                {
                    stateMap[i].SetActive(false);
                }
            }
            collided = false;

            // remove no longer needed objects if fully broken apart
            if (breakState >= splitState)
            {
                breakTimer = null;
                for (int i = 0; i < splitState; i++)
                {
                    Destroy(stateMap[i]);
                }
            }
        }
    }
}
