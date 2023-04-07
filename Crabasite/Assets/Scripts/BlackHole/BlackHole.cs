using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script is used to attract every object in the first Level to the Black Hole if the player took to long
 */
public class BlackHole : MonoBehaviour
{

    private List<Rigidbody2D> pulledObjects = new List<Rigidbody2D>();
    private Rigidbody2D spaceShipRigidBody;
    [SerializeField] private int secondsBeforeBlackHoleIsActive = 480;
    [SerializeField] private int secondsUntilTargetPosition = 15;
    [SerializeField] private Vector3 targetPosition;
    private Vector3 startingPosition;
    private Vector3 fullPath;
    private const float APPROX_MAX_DISTANCE = 60.0f;
    public bool isActive = false;
    private float loadedTime;
    private bool halfTimeMessageSent = false;
    private bool tooLateMessageSent = false;

    private float blackHoleAttractionSpeed(Rigidbody2D rigidbody, float distanceToBlackHole)
    {
        // attract faster, the closer objects are. Clamp to range:
        float speed = Mathf.Clamp(APPROX_MAX_DISTANCE / distanceToBlackHole, 1.0f, 10.0f);
        // attract all object at roughly the same speed
        speed *= Mathf.Clamp(rigidbody.mass, 1.0f, Mathf.Infinity);
        // increase speed over the first 10 seconds after the black hole becomes active
        float timeFactor = Mathf.Clamp(timeSinceActive() / 10, 0.0f, 1.0f);
        speed *= timeFactor;
        // flat factor
        speed *= 0.5f;
        return speed;
    }

    void Start()
    {
        loadedTime = Time.time;
        this.isActive = false;
        startingPosition = transform.position;
        spaceShipRigidBody = GameObject.FindGameObjectWithTag("ShipHull").GetComponent<Rigidbody2D>();
        fullPath = targetPosition - startingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= secondsBeforeBlackHoleIsActive / 2 && !halfTimeMessageSent) { SendHalfTimeMessageToPlayer(); halfTimeMessageSent = true; }
        if (Time.time >= secondsBeforeBlackHoleIsActive - 3 && !tooLateMessageSent) { SendTooLateMessageToPlayer(); tooLateMessageSent = true; }

        if (Time.time < secondsBeforeBlackHoleIsActive) { return; }
        if (!this.isActive)
        {
            this.isActive = true;
            InitPulledObjectList();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Rigidbody2D>().drag = 0.0f;
        }

        updatePosition();

        foreach (Rigidbody2D attractedObject in pulledObjects)
        {
            Vector2 direction = (Vector2)transform.position - attractedObject.position;
            float distanceToBlackHole = direction.magnitude;
            direction.Normalize();
            float speed = blackHoleAttractionSpeed(attractedObject, distanceToBlackHole);

            if (attractedObject.tag == "Player")
            {
                attractedObject.AddForce(direction * speed * 10.0f); // there is no escape
            }
            else
            {
                attractedObject.AddForce(direction * speed);
            }
        }
    }

    void InitPulledObjectList()
    {
        pulledObjects.Clear();
        pulledObjects.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>());
        GameObject levelObjects = GameObject.Find("level objects");
        Rigidbody2D[] rigidBodies = levelObjects.GetComponentsInChildren<Rigidbody2D>();
        for (int i = 0; i < rigidBodies.Length; i++)
        {
            if (rigidBodies[i].name != gameObject.name)
            {
                pulledObjects.Add(rigidBodies[i]);
            }
        }
    }

    private void updatePosition()
    {
        // 0 - 1 how far should we be
        float positionFactor = Mathf.Clamp(timeSinceActive() / secondsUntilTargetPosition, 0.0f, 1.0f);
        transform.position = startingPosition + (fullPath * positionFactor);
    }

    private float levelTime() { return Time.time - loadedTime; }
    private float timeSinceActive() { return levelTime() - secondsBeforeBlackHoleIsActive; }
    private void SendHalfTimeMessageToPlayer() { CommentarySystem.displayProtagonistComment("blackHoleHalftimeWarning"); }
    private void SendTooLateMessageToPlayer() { CommentarySystem.displayProtagonistComment("blackHoleTooLate"); }

    // unused construct to test if the spaceship entered the black hole: 
    // private void OnTriggerEnter2D(Collider2D collider) { if (collider.gameObject.tag == "ShipHull") { Debug.LogWarning("Spaceship passed the black holes event horizon!"); } }
}
