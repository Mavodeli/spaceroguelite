using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    [Header("Inventory Settings")]
    public GameObject Inventory;
    public bool inventoryIsOpened;

    public static InventoryManager Instance;
    public List<string> Items = new List<string>();
    public List<string> Mails = new List<string>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameObject InventoryItemDescription;

    public Transform MailContent;
    public GameObject InventoryMail;
    public GameObject InventoryMailDescription;

    public Transform InventoryDescriptionContent;
    public Transform MailDescriptionContent;


    void Start() {
        inventoryIsOpened = false;
        Inventory.SetActive(false);
    }

    /// If the player presses the "i" key, the inventory will open or close
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


   /// This function is used to list all the items in the inventory tab
    public void ListItems()
    {
        /* Destroying all the items in the inventory and then adding all the items from the list to the
        inventory. */
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        /* Creating a new object for each item in the list. */
        foreach(var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemDescription = obj.transform.Find("ItemDescription").GetComponent<TMP_Text>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemDescription.text = item.description;
        }

    }

    /// It takes a string as a parameter, finds the item with the same name as the string, and then
    /// displays the item's name, icon, and description in the inventory description panel
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

    /// This function is used to list all the mails in the mail tab
    public void ListMails()
    {
        /* Destroying all the mails in the mail tab and then adding all the mails from the list to the
        mail tab. */
        foreach (Transform mail in MailContent)
        {
            Destroy(mail.gameObject);
        }

        /* Creating a new object for each mail in the list. */
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
    }

      /// It takes a string as a parameter, finds the mail with the same name as the string, and then
      /// displays the mail's name, icon, and description
      public void ShowMails(string toFind)
    {
        Destroy(MailDescriptionContent.GetChild(0).gameObject);

        Mail mail = Mails.Find(x => x.mailName == toFind);

        GameObject obj1 = Instantiate(InventoryMailDescription, MailDescriptionContent);
        var mailName = obj1.transform.Find("MailName").GetComponent<TMP_Text>();
        var mailIcon = obj1.transform.Find("MailIcon").GetComponent<Image>();
        var mailDescription = obj1.transform.Find("MailDescription").GetComponent<TMP_Text>();

        mailName.text = mail.mailName;
        mailIcon.sprite = mail.icon;
        mailDescription.text = mail.description;
    }
     public void LoadData(GameData data)
    {
        //
    }
    public void SaveData(ref GameData data)
    {
       data.Mails = this.Mails;
    }

}
