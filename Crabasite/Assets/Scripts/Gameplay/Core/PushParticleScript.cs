using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushParticleScript : MonoBehaviour
{
    [SerializeField]
    private float zPosition = 5;

    [SerializeField]
    private float distanceScaling = 0.6f;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionVector =
            Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;

        Vector2 targetVector = directionVector.normalized * distanceScaling;

        float zRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        // correct orientation of particle system
        zRotation -= 90;

        transform.SetLocalPositionAndRotation(
            new Vector3(targetVector.x, targetVector.y, zPosition),
            Quaternion.Euler(0.0f, 0.0f, zRotation)
        );
    }
}
