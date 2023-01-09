using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionDelegate : MonoBehaviour
{
    public delegate void Function();

    protected void send_mail(string id){
        InventoryManager IM = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
        Mail mail = Instantiate(Resources.Load<Mail>("Scriptable Objects/Mails/"+id));
        IM.Add(mail);
    }
}