using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItemController : MonoBehaviour
{
    public Item item;

    public void Show()
    {
        InventoryManager.Instance.ShowItems(this.transform.Find("ItemName").GetComponent<TMP_Text>().text);
    }
}
