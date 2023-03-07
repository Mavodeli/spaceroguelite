using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script is used to attract every object in the first Level to the Black Hole
 */
public class BlackHole : MonoBehaviour
{

    private List<Rigidbody2D> pulledObjects = new List<Rigidbody2D>();
    private Vector2 blackHolePosition;
    private Rigidbody2D blackHole;
    private Rigidbody2D spaceShip; // the ship hull only
    private Transform spaceShipEntrance;
    private Vector2 spaceShipEntranceRelativeToSpaceship; // space ship position + this vector = space ship entrance position

    private int secondsBeforeBlackHoleIsActive = 30;

    private const float MAX_ATTRACTION_SPEED = 0.18f;
    private const float MIN_ATTRACTION_SPEED = 0.02f;
    public float SPEED_MULTIPLIER = 6f;

    /**
     * Function used to calculate the speed at which an object should be attracted to the black hole, given a certain distance
     *
     * INPUT: The Distance from Object to Black Hole
     * OUTPUT: The Speed of the attraction
     *
     * The speed of the attraction can be used as scalar to a normalized direction vector to transform the object
     * which is attracted by the black hole
     */
    private System.Func<float, float> blackHoleAttractionSpeed = (x) => {
        float result = (-0.08f / 75f) * x + 0.18f;
        result = result > MAX_ATTRACTION_SPEED ? MAX_ATTRACTION_SPEED : result < MIN_ATTRACTION_SPEED ? MIN_ATTRACTION_SPEED : result;
        return result;
    };

    void Start()
    {
        GameObject blackHoleGameObject = GameObject.Find("PH black hole");
        blackHole = (Rigidbody2D)(blackHoleGameObject.GetComponent(typeof(Rigidbody2D)));
        blackHolePosition = blackHole.position;

        spaceShip = (Rigidbody2D)GameObject.Find("PH ship hull").GetComponent(typeof(Rigidbody2D));
        spaceShipEntrance = GameObject.Find("SpaceshipEntrance").transform;
        spaceShipEntranceRelativeToSpaceship = (Vector2) spaceShipEntrance.position - spaceShip.position;

        InitPulledObjectList();

    }

    private bool messageToPlayerWasSent = false;

    // Update is called once per frame
    void Update()
    {
        if(Time.time < secondsBeforeBlackHoleIsActive){
            return;
        }
        if(Time.time >= secondsBeforeBlackHoleIsActive && !messageToPlayerWasSent){
            // TODO send e-mail/message to player
            messageToPlayerWasSent = true;
            Debug.LogWarning("Black Hole is now actively pulling objects towards itself");
            return;
        }

        foreach(Rigidbody2D attractedObject in pulledObjects)
        {
            Vector2 direction = blackHolePosition - attractedObject.position;
            float distanceToBlackHole = direction.magnitude;
            direction.Normalize();
            float speed = blackHoleAttractionSpeed(distanceToBlackHole);
            speed *= SPEED_MULTIPLIER;

            attractedObject.velocity = new Vector2(direction.x, direction.y) * speed;
            if(attractedObject.name == "PH ship hull"){
                spaceShipEntrance.SetPositionAndRotation(attractedObject.position + spaceShipEntranceRelativeToSpaceship, Quaternion.identity);
                // TODO check collision with black hole
            }
        }
    }

    void InitPulledObjectList(){
        pulledObjects.Clear();
        GameObject levelObjects = GameObject.Find("level objects");
        Component[] rigidBodies = levelObjects.GetComponentsInChildren(typeof(Rigidbody2D));
        for(int i = 0; i < rigidBodies.Length; i++){
            Rigidbody2D temp = (Rigidbody2D)rigidBodies[i];
            if(temp.name == "PH black hole"){
                continue;
            }
            pulledObjects.Add(temp);
        }
    }

}
