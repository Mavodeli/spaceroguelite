using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClaw : MonoBehaviour
{
    [SerializeField] private float PullSpeed = 4;
    [SerializeField] private float PushSpeed = 4;
    [SerializeField] private float range = 10;
    [SerializeField] private GameObject PushParticleSystemPrefab;
    [SerializeField] private GameObject PullParticleSystemPrefab;
    private LayerMask DetectionLayer;

    Rigidbody2D objectRigidbody;

    private PlayerMana PM;
    protected GameObject player;
    private TimerObject manaCooldown;

    private GameObject inventory;
    private GameObject soundController;
    private ParticleSystem PushParticleSystem;
    private Timer PullParticleCooldown;


    void Start(){
        DetectionLayer = LayerMask.GetMask("Raycast");
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMana>();
        manaCooldown = new TimerObject("Player manaCooldown");
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        soundController = GameObject.Find("Sounds");
        PullParticleCooldown = new Timer();
        // Instantiate Push Particle System as child
        GameObject PushParticleObject = Transform.Instantiate(PushParticleSystemPrefab);
        PushParticleObject.transform.SetParent(gameObject.transform, false);
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
        //- maximal t: range
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos_relative_to_player, range, DetectionLayer);
        if(hit.collider != null && !inventory.GetComponent<InventoryManager>().inventoryIsOpened){ // check if inventory is off to enable crab claw 
            //playerPos_relative_to_hit: vector that points [location where the ray hits a collider] -> Player
            Vector2 playerPos_relative_to_hit = transform.position-hit.transform.position;
            playerPos_relative_to_hit.Normalize();

            //update the position of the object hit by the ray
            if(Input.GetMouseButton(0) && PM.hasMana()){
                attachPullParticleSystem(hit.transform.gameObject);
                objectRigidbody = hit.transform.gameObject.GetComponent<Rigidbody2D>();
                objectRigidbody.velocity = Vector3.zero;
                objectRigidbody.AddForce(playerPos_relative_to_hit*PullSpeed);

                soundController.SendMessage("playSoundLoopingSafe", new SoundParameter("PlayerPullSound", player, 0.5f, false), SendMessageOptions.DontRequireReceiver);

            }

            if(Input.GetMouseButton(1) && PM.hasMana()){
                PushParticleSystem.Play(true);
                objectRigidbody = hit.transform.gameObject.GetComponent<Rigidbody2D>();
                objectRigidbody.velocity = Vector3.zero;
                objectRigidbody.AddForce(mousePos_relative_to_player*PushSpeed);
                // hit.transform.position += PushMI.getFrameDirection()*PushMI.getFrameSpeed()*Time.deltaTime;

                soundController.SendMessage("playSoundLoopingSafe", new SoundParameter("PlayerPushSound", player, 0.5f, false), SendMessageOptions.DontRequireReceiver);

            }
            else{
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

        PullParticleCooldown.Update();
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
        // if the target is constructed of multiple smaller pieces, attach one to each
        if (!PullParticleCooldown.is_running()) 
        {
            GameObject newSystem = Transform.Instantiate(PullParticleSystemPrefab);
            newSystem.transform.SetParent(targetObject.transform, false);
            PullParticleScript pps = newSystem.GetComponent<PullParticleScript>();
            PullParticleCooldown.start(pps.lifeTime); // adapt the cooldown to the lifetime settings of the Particle Script
        }
    }
}
