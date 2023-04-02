using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClaw : MonoBehaviour
{
    [SerializeField] private float PullSpeed = 4;
    [SerializeField] private float PushSpeed = 4;
    [SerializeField] private float StartDelayDuration = 0.15f;
    [SerializeField] private float EndDelayDuration = 0.3f;
    [SerializeField] private GameObject PushParticleSystemPrefab;
    [SerializeField] private GameObject PullParticleSystemPrefab;    
    // [SerializeField] private float PullSafetyDistance = 2;
    //IMPORTANT: the movement interpolation doesn't work for the end delay
    //  (and fixing that would significantly increase the complexity of the script, hence why that is not fixed) 
    private MovementInterpolation PushMI;
    private MovementInterpolation PullMI;
    //the Layer in which the ray checks for intersections (default: 'Enemies')
    private LayerMask DetectionLayer;

    Rigidbody2D objectRigidbody;

    private PlayerMana PM;
    protected GameObject player;
    private TimerObject manaCooldown;

    private GameObject inventory;
    private GameObject soundController;
    private ParticleSystem PushParticleSystem;



    void Awake(){
        PushMI = new MovementInterpolation();
        PullMI = new MovementInterpolation();
        PushMI.Awake();
        PullMI.Awake();
    }

    void Start(){
        PushMI.Start(PushSpeed, StartDelayDuration, EndDelayDuration);
        PullMI.Start(PullSpeed, StartDelayDuration, EndDelayDuration);
        DetectionLayer = LayerMask.GetMask("Raycast");
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMana>();
        manaCooldown = new TimerObject("Player manaCooldown");
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        soundController = GameObject.Find("Sounds");
        // Instantiate Push Particle System as child
        GameObject PushParticleObject = Transform.Instantiate(PushParticleSystemPrefab);
        PushParticleObject.transform.parent = gameObject.transform;
        PushParticleSystem = PushParticleObject.GetComponent<ParticleSystem>();
        PushParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // prevent particles when loading into scene
    }
    
    void Update()
    {   
        //mousePos_relative_to_player: vector that points Player -> Mouse Cursor
        Vector2 mousePos_relative_to_player = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        mousePos_relative_to_player = new Vector2(mousePos_relative_to_player.x, mousePos_relative_to_player.y);
        mousePos_relative_to_player.Normalize();

        //Ray characteristics
        //- origin: Player Position
        //- direction: normalized Mouse Cursor Position
        //- maximal t: +infinity
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos_relative_to_player, Mathf.Infinity, DetectionLayer);
        if(hit.collider != null && !inventory.GetComponent<InventoryManager>().inventoryIsOpened){ // check if inventory is off to enable crab claw 
            //playerPos_relative_to_hit: vector that points [location where the ray hits a collider] -> Player
            Vector2 playerPos_relative_to_hit = transform.position-hit.transform.position;
            playerPos_relative_to_hit.Normalize();

            // float dist = Vector3.Distance(transform.position, hit.transform.position);

            //update the position of the object hit by the ray
            // if(Input.GetMouseButton(0) && dist > PullSafetyDistance){
            if(Input.GetMouseButton(0) && PM.hasMana()){
                attachPullParticleSystem(hit.transform.gameObject);
                PullMI.Update(true, playerPos_relative_to_hit);
                objectRigidbody = hit.transform.gameObject.GetComponent<Rigidbody2D>();
                objectRigidbody.velocity = Vector3.zero;
                objectRigidbody.AddForce(PullMI.getFrameDirection()*PullMI.getFrameSpeed());
                // hit.transform.position += PullMI.getFrameDirection()*PullMI.getFrameSpeed()*Time.deltaTime;

                soundController.SendMessage("playSoundLoopingSafe", new SoundParameter("PlayerPullSound", player, 0.5f, false), SendMessageOptions.DontRequireReceiver);

            }
            else{
                PullMI.Update(false, playerPos_relative_to_hit);
            }

            if(Input.GetMouseButton(1) && PM.hasMana()){
                PushParticleSystem.Play(true);
                PushMI.Update(true, mousePos_relative_to_player);
                objectRigidbody = hit.transform.gameObject.GetComponent<Rigidbody2D>();
                objectRigidbody.velocity = Vector3.zero;
                objectRigidbody.AddForce(PushMI.getFrameDirection()*PushMI.getFrameSpeed());
                // hit.transform.position += PushMI.getFrameDirection()*PushMI.getFrameSpeed()*Time.deltaTime;

                soundController.SendMessage("playSoundLoopingSafe", new SoundParameter("PlayerPushSound", player, 0.5f, false), SendMessageOptions.DontRequireReceiver);

            }
            else{
                PushMI.Update(false, mousePos_relative_to_player);
                PushParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && !PM.hasMana()){
                soundController.SendMessage("playSoundSafe", new SoundParameter("PlayerManaEmpty", player, 1f, false), SendMessageOptions.DontRequireReceiver);
            }

            if(Input.GetMouseButtonUp(0)){
                soundController.SendMessage("stopSound", "PlayerPullSound", SendMessageOptions.DontRequireReceiver);
            }
            if(Input.GetMouseButtonUp(1)){
                soundController.SendMessage("stopSound", "PlayerPushSound", SendMessageOptions.DontRequireReceiver);
            }
            if(!PM.hasMana()){
                soundController.SendMessage("stopSound", "PlayerPullSound", SendMessageOptions.DontRequireReceiver);
                soundController.SendMessage("stopSound", "PlayerPushSound", SendMessageOptions.DontRequireReceiver);
            }

        }
        else{
            PushMI.Update(false, new Vector2(0, 0));
            PullMI.Update(false, new Vector2(0, 0));
        }
    }

       void FixedUpdate()
    {
        if (!inventory.GetComponent<InventoryManager>().inventoryIsOpened)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                PM.addMana(-2);
                manaCooldownTimer(2.5f);
            }
            if (!manaCooldown.runs())
            {
                PM.addMana(1);
            }
        }
    }

    void manaCooldownTimer(float duration)
    {
        if (!manaCooldown.runs())
        {
            manaCooldown.start(duration);
        }
    }

    void attachPullParticleSystem(GameObject targetObject) {
        // if (targetObject.transform.Find("PullAnimation"))
        {
            GameObject newSystem = Transform.Instantiate(PullParticleSystemPrefab);
            newSystem.transform.parent = targetObject.transform;
        }
    }
}
