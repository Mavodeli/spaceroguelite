using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullParticleScript : MonoBehaviour
{
    [SerializeField]
    private float zPosition = 5;

    [SerializeField]
    public float lifeTime = 0.2f;
    private float distanceScaling;
    private GameObject player;
    private Timer lifeTimer;

    void Awake()
    {
        lifeTimer = new Timer();
        // move ourselves far away to prevent glitchy particles before the first frame Update
        transform.SetPositionAndRotation(
            new Vector3(10000, 10000, zPosition),
            new Quaternion(0, 0, 1, 0)
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lifeTimer.start(lifeTime);

        // scale to parent size
        GameObject parent = transform.parent.gameObject;
        Renderer r = transform.parent.GetComponent<Renderer>();
        Bounds parentBounds;
        if (r == null)
        {
            // if the parent has no renderer, it is invalid and we abort.
            Destroy(gameObject);
        }
        parentBounds = r.bounds;

        // scale distance
        distanceScaling = Mathf.Max(parentBounds.extents.x, parentBounds.extents.y) / 3;
        if (distanceScaling < 1.0f)
        {
            distanceScaling = 1.0f;
        }
        if (distanceScaling > 2.0f)
        {
            distanceScaling = 2.0f;
        }

        float scalingFactor = Mathf.Max(parentBounds.extents.x, parentBounds.extents.y) / 2;

        // clamp scalingFactor
        if (scalingFactor < 1.0f)
        {
            scalingFactor = 1.0f;
        }
        if (scalingFactor > 5.0f)
        {
            scalingFactor = 5.0f;
        }

        // Set particle speed antiproportional to scaling factor
        // and scale emission shape width
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startSpeedMultiplier = 1 / scalingFactor;
        var shape = ps.shape;
        shape.radius *= scalingFactor;
        if (shape.radius > 1.0f)
        {
            shape.radius = 1.0f;
        }
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
