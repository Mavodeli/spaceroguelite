using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mavodebug : MonoBehaviour
{
    [SerializeField]
    private GameObject NegativeChargeParticlePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = Transform.Instantiate(NegativeChargeParticlePrefab);
        go.transform.SetParent(transform, false);
        go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update() { }
}
