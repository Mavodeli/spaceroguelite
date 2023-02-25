using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractablesHandler : MonoBehaviour
{

    private AudioSource spaceShipDoor;
    private AudioSource collectUltimate;

    void Start(){

        spaceShipDoor = (AudioSource) (GameObject.Find("SpaceShipDoor")).GetComponent(typeof (AudioSource));
        collectUltimate = (AudioSource) (GameObject.Find("PickupUlt")).GetComponent(typeof (AudioSource));

    }

    void Update()
    {
        
    }
}
