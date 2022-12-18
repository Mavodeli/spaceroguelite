using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemPickup : MonoBehaviour//, IPointerDownHandler
{
    private Item Item;
    private Mail Mail;
    public ScriptableObject target;

    public void Pickup()
    {
        if (target.GetType() == typeof(Item))
        {   
            Item = (Item)target;
            InventoryManager.Instance.Add(Item);
        }
        else if (target.GetType() == typeof(Mail))
        {
            Mail = (Mail)target;
            InventoryManager.Instance.Add(Mail);
        }
        Destroy(gameObject);
    }
}
