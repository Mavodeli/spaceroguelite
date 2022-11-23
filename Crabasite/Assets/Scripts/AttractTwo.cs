using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttractTwo : Ultimate
{
    GameObject player;
    
    public GameObject selectedObject1;
    public GameObject selectedObject2;

    public float object1Speed;
    public float object2Speed;

    const float SPEED = 5;

    public KeyCode key;

    //selects two targets and adds a script to both of them, making them pull together.
    //needed to be split in two (AttractTwo and AttractTwoBehaviour) so the pulling of the objects happens separately
    //and we don't have to wait for them to collide to start the cooldown
    //TODO: make it do dmg
    public override void Use()
    {
        //select two targets
        SelectTargets();

        //when both target are selected it assigns the pull scripts to them and resets all fields
        if(selectedObject1 != null && selectedObject2 != null)
        {
            //attach the pull script
            AttractTwoBehaviour object1Script = selectedObject1.AddComponent<AttractTwoBehaviour>();
            AttractTwoBehaviour object2Script = selectedObject2.AddComponent<AttractTwoBehaviour>();

            //assign each other as the target to pull towards
            object1Script.target = selectedObject2;
            object2Script.target = selectedObject1;

            //assign the speed of the pull
            object1Script.speed = SPEED;
            object2Script.speed = SPEED;

            //resets all fields for next activation of the ultimate
            isActive = false;
            selectedObject1 = null;
            selectedObject2 = null;
        }     
    }

    //selects a gameobject via raycast and returns it
    //returns   gameobject that was selected
    GameObject SelectTarget()
    {
        GameObject target = null;
        player = GameObject.Find("Player");
        Vector3 playerPosition = player.GetComponent<Transform>().position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 playerToMouseDirection = new Vector3(mousePosition.x - playerPosition.x, mousePosition.y - playerPosition.y, 0);
        playerToMouseDirection.Normalize();

        RaycastHit hit;
        if (Input.GetKeyDown(key))
        {
            if (Physics.Raycast(playerPosition, playerToMouseDirection, out hit))
            {              
                target = GameObject.Find(hit.transform.name);
            }           
        }

        return target;
    }

    //selects both targets for the ultimate
    void SelectTargets()
    {
        //only look for a target if the first selected object is still null
        if (selectedObject1 == null)
        {
            selectedObject1 = SelectTarget();
        }

        //only look for a second target if the second selected object is still null
        if (selectedObject2 == null)
        {
            //only assign the selected object if the found target is not the same gameobject as the first
            GameObject selectedObject = SelectTarget();
            if (selectedObject1 != selectedObject)
            {
                selectedObject2 = selectedObject;
            }
        }
    }
}
