using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RayCastSelect
{
    public static GameObject SelectTarget(KeyCode key)
    {
        GameObject target = null;
        GameObject player = GameObject.Find("Player");
        //get positions for mouse and player
        Vector3 playerPosition = player.GetComponent<Transform>().position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //save the vector that goes from the player to the mouse and normalize it to make it a direction vector
        Vector3 playerToMouseDirection = new Vector3(mousePosition.x - playerPosition.x, mousePosition.y - playerPosition.y, 0);
        playerToMouseDirection.Normalize();

        //hit will store information about the raycast hit
        RaycastHit2D hit = Physics2D.Raycast(playerPosition, playerToMouseDirection, Mathf.Infinity, LayerMask.GetMask("Enemies"));
        //if the key is down and the raycast hits something then store the hit gameobject inside target
        if (Input.GetKeyDown(key))
        {
            //raycast from player to mouse, returns true if it hits something
            //out hit stores information about what the raycast hit
            // Debug.Log(hit.collider);
            if (hit.collider != null)
            {
                //get the name of the gameobject from the info stored in hit and find the gameobject to store it inside target
                target = GameObject.Find(hit.transform.name);
            }
        }
        return target;
    }
}
