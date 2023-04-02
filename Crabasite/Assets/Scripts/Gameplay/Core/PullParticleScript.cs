using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullParticleScript : MonoBehaviour
{
    [SerializeField]
    private float zPosition = 5;

    [SerializeField]
    private float distanceScaling = 0.5f;

    [SerializeField]
    public float lifeTime = 0.2f;
    private GameObject player;
    private Timer lifeTimer;

    void Awake()
    {
        lifeTimer = new Timer();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lifeTimer.start(lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionVector = player.transform.position - transform.parent.position;

        Vector2 targetVector = directionVector.normalized * distanceScaling;

        float zRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        // correct orientation of particle system
        zRotation -= 90;

        transform.SetPositionAndRotation(
            new Vector3(
                transform.parent.position.x + targetVector.x,
                transform.parent.position.y + targetVector.y,
                zPosition
            ),
            Quaternion.Euler(0.0f, 0.0f, zRotation)
        );

        // Timer
        lifeTimer.Update();
        // End of Life?
        if (!lifeTimer.is_running())
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            StartCoroutine(DelayedDestroy());
        }
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.5f); // this needs to match maximum particle lifetime
        Destroy(gameObject);
    }
}
