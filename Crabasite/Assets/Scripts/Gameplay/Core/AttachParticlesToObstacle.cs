using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachParticlesToObstacle : MonoBehaviour
{
    [SerializeField]
    private float zPosition = 5;

    [SerializeField]
    private float distanceScaling = 0.5f;
    private GameObject player;

    // Start is called before the first frame update
    void Start() { 
        player = GameObject.FindGameObjectWithTag("Player");
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
    }
}
