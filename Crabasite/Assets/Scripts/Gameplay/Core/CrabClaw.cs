using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClaw : MonoBehaviour
{
    [SerializeField] private float PullSpeed = 1000;
    [SerializeField] private float PushSpeed = 1000;
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


    void Start()
    {
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
        Vector2 mousePos_relative_to_player = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mousePos_relative_to_player = new Vector2(mousePos_relative_to_player.x, mousePos_relative_to_player.y);
        mousePos_relative_to_player.Normalize();

        // only cast ray if mouse is being pressed, inventory is closed and the player has mana
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && !inventory.GetComponent<InventoryManager>().inventoryIsOpened)
        {
            // check if the player has mana
            if (!PM.hasMana())
            {
                soundController.SendMessage("playSoundSafe", new SoundParameter("PlayerManaEmpty", player, 1f, false), SendMessageOptions.DontRequireReceiver);
            }

            // Raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos_relative_to_player, range, DetectionLayer);
            if (hit.collider != null)
            {
                Vector2 playerPos_relative_to_hit = transform.position - hit.transform.position;
                playerPos_relative_to_hit.Normalize();

                // pull
                if (Input.GetMouseButton(0))
                {
                    // particles
                    attachPullParticleSystem(hit.transform.gameObject);
                    // apply force
                    objectRigidbody = hit.transform.gameObject.GetComponent<Rigidbody2D>();
                    objectRigidbody.AddForce(playerPos_relative_to_hit * PullSpeed);
                    // play sound
                    soundController.SendMessage("playSoundLoopingSafe", new SoundParameter("PlayerPullSound", player, 0.5f, false), SendMessageOptions.DontRequireReceiver);
                }

                // push
                if (Input.GetMouseButton(1))
                {
                    // particles
                    PushParticleSystem.Play(true);
                    // apply force
                    objectRigidbody = hit.transform.gameObject.GetComponent<Rigidbody2D>();
                    objectRigidbody.AddForce(mousePos_relative_to_player * PushSpeed);
                    // play sound
                    soundController.SendMessage("playSoundLoopingSafe", new SoundParameter("PlayerPushSound", player, 0.5f, false), SendMessageOptions.DontRequireReceiver);
                }
            }
            else // no hit
            {
                stopAll();
            }
        }
        else // no input, mana or inventory opened
        {
            stopAll();
        }

        PullParticleCooldown.Update();
    }

    private void stopAll()
    {
        // particles
        PushParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        // sounds
        soundController.SendMessage("stopSound", "PlayerPullSound", SendMessageOptions.DontRequireReceiver);
        soundController.SendMessage("stopSound", "PlayerPushSound", SendMessageOptions.DontRequireReceiver);
    }

    void FixedUpdate()
    {
        if (!inventory.GetComponent<InventoryManager>().inventoryIsOpened)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                PM.addMana(-2);
                manaCooldownTimer(1.5f);
            }
            if (!manaCooldown.runs())
            {
                PM.addMana(3);
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

    void attachPullParticleSystem(GameObject targetObject)
    {
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
