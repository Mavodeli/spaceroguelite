using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemPickup : MonoBehaviour, IPointerDownHandler
{
    public Item Item;

    void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }


    private void OnMouseDown()
    {
        Pickup();
        Debug.Log("MouseEvent");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pickup();
        Debug.Log("PointerDown");
        
    }
}
