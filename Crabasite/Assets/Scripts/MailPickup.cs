using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailPickup : MonoBehaviour
{
    public Mail Mail;

    void Pickup()
    {
        InventoryManager.Instance.Add(Mail);
        Destroy(gameObject);
    }


    private void OnMouseDown()
    {
        Pickup();
    }
}
