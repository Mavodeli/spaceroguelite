using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractionButton : MonoBehaviour
{
    private float distanceToPlayer = Mathf.Infinity;
    private float showDistanceMaximum;
    private GameObject player;
    private GameObject button;
    private Vector3 offset = new Vector3(1, 1, 0);
    private bool hasButton = false;
    private string inputKey = "e";
    public delegate void OnButtonPressDelegate();
    private OnButtonPressDelegate onButtonPress;
    public delegate void OnNotInteractableButtonPressDelegate();
    private OnNotInteractableButtonPressDelegate onNotInteractableButtonPress;
    private bool lastFrameGetKey;
    private TimerObject buttonPressCooldown;
    private bool isVisible;
    private bool isInteractable;

    public void Setup(OnButtonPressDelegate _delegate, string _inputKey = "e", float _showDistanceMaximum = 3, bool visible = true, bool interactable = true, OnNotInteractableButtonPressDelegate _otherDelegate = null){
        onButtonPress = _delegate;
        onNotInteractableButtonPress = _otherDelegate == null ? delegate(){CommentarySystem.displayProtagonistComment("Default_IsNotInteractable");} : _otherDelegate;
        inputKey = _inputKey;
        showDistanceMaximum = _showDistanceMaximum;
        buttonPressCooldown = new TimerObject();
        isVisible = visible;
        isInteractable = interactable;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        button = Resources.Load<GameObject>("Prefabs/Inventory/button");
        lastFrameGetKey = false;
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //create button if not present and player is close enough
        if ((distanceToPlayer <= showDistanceMaximum) && !hasButton && isVisible)
        {
            button.name = "InteractionButton of " + name;
            button.GetComponent<SpriteRenderer>().sortingOrder = 3;
            Instantiate(button, transform.position + offset, Quaternion.identity, transform);
            hasButton = true;
        }
        //destroy button if player is too far away
        if ((distanceToPlayer > showDistanceMaximum) && hasButton)
        {
            Destroy(transform.Find("InteractionButton of " + name + "(Clone)").gameObject);
            hasButton = false;
        }
        //check if player presses button, if so, perform onButtonPress
        if (Input.GetKey(inputKey) && !lastFrameGetKey && hasButton && !buttonPressCooldown.runs() && isVisible && isInteractable)
        {
            buttonPressCooldown.start(1.5f);
            onButtonPress();
        }
        else if(Input.GetKey(inputKey) && !lastFrameGetKey && hasButton && !buttonPressCooldown.runs() && isVisible && !isInteractable){
            buttonPressCooldown.start(1.5f);
            onNotInteractableButtonPress();
        }
        lastFrameGetKey = Input.GetKey(inputKey);
    }

    public void setNewOffset(Vector3 newOffset){
        offset = newOffset;
    }

    public void setVisibility(bool value){
        isVisible = value;
    }

    public void setInteractability(bool value){
        isInteractable = value;
    }

    //pass-through function used by the EmergencyDoor
    public static void displayProtagonistComment(string id){
        CommentarySystem.displayProtagonistComment(id);
    }

    //pass-through function used by the EmergencyDoor
    public static void displayAIComment(string id){
        CommentarySystem.displayAIComment(id);
    }
}