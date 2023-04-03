using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Scale circle to roughly fit parent bounds
        GameObject parent = transform.parent.gameObject;
        Renderer parentRenderer = transform.parent.GetComponent<Renderer>();
        if (parentRenderer == null)
        {
            // if the parent has no renderer, it is invalid and we abort.
            Destroy(gameObject);
        }
        Bounds parentBounds = parentRenderer.bounds;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        var shape = ps.shape;
        // radius is half of the average size of the parent bounds
        shape.radius = (parentBounds.size.x + parentBounds.size.y) / 4;
        // scale emission with size of the circle
        var emission = ps.emission;
        emission.rateOverTimeMultiplier *= shape.radius;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, -90, -90));
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.5f); // this needs to match maximum particle lifetime
        Destroy(gameObject);
    }

    public void DestroyParticleObject()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(DelayedDestroy());
    }
}
