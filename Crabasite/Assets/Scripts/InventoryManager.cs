using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    public bool inventoryIsOpened;

    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public List<Mail> Mails = new List<Mail>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Transform MailContent;
    public GameObject InventoryMail;



    void Start() {
        inventoryIsOpened = false;
    }

    void Update() {
        if (Input.GetKeyDown("i")) {
            if (inventoryIsOpened == false) {
                Inventory.SetActive(true);
                inventoryIsOpened = true;

                ListItems();
                ListMails();
                
            }
            else {
                Inventory.SetActive(false);
                inventoryIsOpened = false;
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void Add(Mail mail)
    {
        Mails.Add(mail);
    }

    public void Remove(Mail mail)
    {
        Mails.Remove(mail);
    }


    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        // gets all items from the List and puts them inside the inventory
        foreach(var item in Items)
        {
            GameObject obj1 = Instantiate(InventoryItem, ItemContent);
            var itemName = obj1.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj1.transform.Find("ItemIcon").GetComponent<Image>();

            Debug.Log(Items[0]);
            

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }

    }

    public void ListMails()
    {
        foreach (Transform mail in MailContent)
        {
            Destroy(mail.gameObject);
        }

        foreach(var mail in Mails)
        {
            GameObject obj2 = Instantiate(InventoryMail, MailContent);
            var mailName = obj2.transform.Find("MailName").GetComponent<TMP_Text>();
            var mailIcon = obj2.transform.Find("MailIcon").GetComponent<Image>();

            
            
            

            mailName.text = mail.mailName;
            mailIcon.sprite = mail.icon;
        }

        Debug.Log(Mails.Count);

    }

}
