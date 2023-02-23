using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractionButton : MonoBehaviour
{
    private float distanceToPlayer = Mathf.Infinity;
    private float showDistanceMaximum = 3;
    private GameObject player;
    private GameObject button;
    private Vector3 offset = new Vector3(1, 1, 0);
    private bool hasButton = false;
    private string inputKey;
    public delegate void OnButtonPressDelegate();
    private OnButtonPressDelegate onButtonPress;

    public void Setup(OnButtonPressDelegate _delegate, string _inputKey = "e"){
        onButtonPress = _delegate;
        inputKey = _inputKey;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        button = Resources.Load<GameObject>("Prefabs/Inventory/button");
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //create button if not present and player is close enough
        if ((distanceToPlayer <= showDistanceMaximum) && !hasButton)
        {
            button.name = "InteractionButton of " + name;
            button.GetComponent<SpriteRenderer>().sortingOrder = 1;
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

        if (Input.GetKey(inputKey) && hasButton)
        {
            onButtonPress();
        }
    }

    public void setNewOffset(Vector3 newOffset){
        offset = newOffset;
    }
}