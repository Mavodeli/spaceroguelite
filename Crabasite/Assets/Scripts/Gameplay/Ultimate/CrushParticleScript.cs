using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushParticleScript : MonoBehaviour
{
    private Timer lifeTimer;

    void Awake()
    {
        lifeTimer = new Timer();
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        lifeTimer.start(ps.main.duration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // stick to the player like glue
        lifeTimer.Update();
        if (!lifeTimer.is_running())
        {
            Destroy(gameObject);
        }
    }
}
