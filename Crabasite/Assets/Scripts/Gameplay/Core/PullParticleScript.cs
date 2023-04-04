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
            // Are there any valid children?
            Renderer[] childRenderers = parent.GetComponentsInChildren<Renderer>();
            if (childRenderers.Length < 1)
            {
                // if there aren't we abort
                Destroy(gameObject);
            }
            // if there are we calculate bounds from them:
            parentBounds = GetBoundsFromActiveChildren(parent, childRenderers);
        }
        else
        {
            parentBounds = r.bounds;
        }

        // scaling
        float averageParentSide = (parentBounds.size.x + parentBounds.size.y) / 2;
        // scale distance
        distanceScaling = (averageParentSide / 2) * 0.75f;

        float scaling = averageParentSide * (1.0f / 3.0f);
        // scale emission shape width
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var shape = ps.shape;
        shape.radius = scaling / 2;
        // scale emission rate to shape size
        var emission = ps.emission;
        emission.rateOverTimeMultiplier *= shape.radius;
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

    Bounds GetBoundsFromActiveChildren(GameObject parent, Renderer[] childRenderers)
    {
        if (childRenderers.Length == 1)
        {
            // if only one child is valid, copy its bounds
            return childRenderers[0].bounds;
        }
        else
        {
            // otherwise combine bounds of all children
            Bounds bounds = childRenderers[0].bounds;
            for (int i = 1; i < childRenderers.Length; i++)
            {
                GameObject childObject = childRenderers[i].transform.gameObject;
                if (childObject.activeSelf && !childObject.GetComponent<PullParticleScript>())
                {
                    bounds.Encapsulate(childRenderers[i].bounds);
                }
            }
            return bounds;
        }
    }
}
