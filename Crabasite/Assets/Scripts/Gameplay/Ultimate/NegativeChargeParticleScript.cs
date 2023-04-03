using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeChargeParticleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Scale emission shape to parent bounds
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
        // circle should be inside collider of most shapes
        shape.radius = Mathf.Min(parentBounds.size.x / 2, parentBounds.size.y / 2) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }
}
