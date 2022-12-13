using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Health Settings")]
    public GameObject Inventory;
    public bool inventoryIsOpened;

    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public List<Mail> Mails = new List<Mail>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameObject InventoryItemDescription;

    public Transform MailContent;
    public GameObject InventoryMail;

    public Transform InventoryDescriptionContent;
    public Transform MailDescriptionContent;


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
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemDescription = obj.transform.Find("ItemDescription").GetComponent<TMP_Text>();

            Debug.Log(Items[0]);
            

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemDescription.text = item.description;
        }

    }

    public void ShowItems(string toFind)
    {
        Destroy(InventoryDescriptionContent.GetChild(0).gameObject);

        Item item = Items.Find(x => x.itemName == toFind);

        GameObject obj1 = Instantiate(InventoryItemDescription, InventoryDescriptionContent);
        var itemName = obj1.transform.Find("ItemName").GetComponent<TMP_Text>();
        var itemIcon = obj1.transform.Find("ItemIcon").GetComponent<Image>();
        var itemDescription = obj1.transform.Find("ItemDescription").GetComponent<TMP_Text>();

        itemName.text = item.itemName;
        itemIcon.sprite = item.icon;
        itemDescription.text = item.description;
    }

    public void ListMails()
    {
        foreach (Transform mail in MailContent)
        {
            Destroy(mail.gameObject);
        }

        foreach(var mail in Mails)
        {
            GameObject obj = Instantiate(InventoryMail, MailContent);
            var mailName = obj.transform.Find("MailName").GetComponent<TMP_Text>();
            var mailIcon = obj.transform.Find("MailIcon").GetComponent<Image>();
            var mailDescription = obj.transform.Find("MailDescription").GetComponent<TMP_Text>();
            

            mailName.text = mail.mailName;
            mailIcon.sprite = mail.icon;
            mailDescription.text = mail.description;
        }

        Debug.Log(Mails.Count);

    }

}
