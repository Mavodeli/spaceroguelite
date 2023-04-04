using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractTwoParticleScript : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private float distanceScaling;
    private GameObject player;

    void Awake()
    {
        // move ourselves far away to prevent glitchy particles before the first frame Update
        transform.SetPositionAndRotation(
            new Vector3(10000, 10000, transform.position.z),
            new Quaternion(0, 0, 1, 0)
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

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
        // if we have no target anymore (fading out state) we don't update our position anymore
        if (!target || !target.transform || !target.activeSelf)
        {
            return;
        }
        Vector2 direction = target.transform.position - transform.parent.position;
        Vector2 positionVector = direction.normalized * distanceScaling;

        float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // correct orientation of particle system
        zRotation -= 90;

        transform.SetPositionAndRotation(
            new Vector3(
                transform.parent.position.x + positionVector.x,
                transform.parent.position.y + positionVector.y,
                transform.position.z
            ),
            Quaternion.Euler(0.0f, 0.0f, zRotation)
        );
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
                if (childObject.activeSelf && !childObject.GetComponent<AttractTwoParticleScript>())
                {
                    bounds.Encapsulate(childRenderers[i].bounds);
                }
            }
            return bounds;
        }
    }

    public void DestroyParticleObject()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(DelayedDestroy());
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
