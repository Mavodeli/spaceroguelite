using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// Controller for the Mail Prefab
public class InventoryMailController : MonoBehaviour
{
    public Mail mail;

    public void Show()
    {
        InventoryManager.Instance.ShowMail(this.transform.Find("MailName").GetComponent<TMP_Text>().text);
    }
}
