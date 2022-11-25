using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelBehaviour : MonoBehaviour
{
    Vector3 _distance;
    public GameObject infusedObject;
    Vector3 _position;
    float _force;
    Rigidbody myRigidbody;
    SphereCollider infusedObjectCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        //get the rigidbody to be able to apply force
        myRigidbody = gameObject.GetComponent<Rigidbody>();
        //get the collider of the negatively charged object to be able to meassure the distance to it
        infusedObjectCollider = infusedObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the collider was not the destroyed (it is destroyed after it's duration)
        if(infusedObjectCollider != null)
        {
            //get the distance between the two objects to be able to apply the different force, depending on how close the object is to the negative charge
            _position = gameObject.transform.position;
            _distance = new Vector3(infusedObject.transform.position.x - _position.x, infusedObject.transform.position.y - _position.y, 0);
            //need the direction to apply force later
            Vector3 direction = _distance.normalized;
            //force is calculated with a log function that returns values between [0,1) when given positive distance values
            _force = (float)(((Mathf.Log(_distance.magnitude + 1) * 0.5)+2)*0.5) * 1;

            //if the object is inside the negative charge radius, apply force to it to repel it
            if (_distance.magnitude <= infusedObjectCollider.radius)
            {
                myRigidbody.AddForce(-direction * _force);
            } else // if it left the radius, destroy this script
            {
                Destroy(GetComponent<RepelBehaviour>());
            }
        }       
        
    }
}
