using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    [Header("Inventory Settings")]
    public GameObject Inventory;
    public bool inventoryIsOpened;

    public static InventoryManager Instance;
    public List<string> Items = new List<string>();
    public List<string> Mails = new List<string>();

    public Dictionary<string,int> ItemDict= new Dictionary<string,int>();
    public Dictionary<string,bool> MailDict= new Dictionary<string, bool>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameObject InventoryItemDescription;

    public Transform MailContent;
    public GameObject InventoryMail;
    public GameObject InventoryMailDescription;

    public Transform InventoryDescriptionContent;
    public Transform MailDescriptionContent;

    public Transform UltimatesContent;

    private Dictionary<int, bool> UltimateDict;

    private GameObject GameHandler;

    void Start() {
        inventoryIsOpened = false;
        Inventory.SetActive(false);
        GameHandler = GameObject.FindGameObjectWithTag("GameHandler");
    }

    /// If the player presses the "i" key, the inventory will open or close
    void Update() {
        if (Input.GetKeyDown("i")) {
            if (inventoryIsOpened == false) {
                Inventory.SetActive(true);
                inventoryIsOpened = true;

                ListItems();
                ListMails();
                updateUltimateButtons();
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

    public bool ItemInDict(string name)
    {
        bool ItemExists = true;
        try
        {
            int amount = ItemDict[name]; 
        }
        catch (KeyNotFoundException)
        {
            ItemExists = false;
        }

        return ItemExists;
    }

    public int ItemAmountInDict(string name){
        int amount = 0;
        if(ItemInDict(name))
            amount = ItemDict[name];
        return amount;
    }

    public void AddItem(string name, int amount = 1)
    {
        try
        {
            ItemDict[name] += amount;
        }
        catch (KeyNotFoundException)
        {
            ItemDict.Add(name, amount);
        }
        GameHandler.SendMessage("InvokeEvent_moveItemToInventory", SendMessageOptions.DontRequireReceiver);
    }

    public void RemoveItem(string name, int amount = 1)
    {
        try
        {
            ItemDict[name] -= amount;
            if (ItemDict[name] < 1) ItemDict.Remove(name);
        }
        catch (KeyNotFoundException)
        {
            return;
        }
    }

    public bool MailInDict(string name)
    {
        bool MailExists;
        try
        {
            MailExists = MailDict[name];    
        }
        catch (KeyNotFoundException)
        {
            MailExists = false;
        }
        return MailExists;
    }

    public void AddMail(string name)
    {
        MailDict.Add(name, true);
    }

    public void RemoveMail(string name)
    {
        try
        {
            MailDict.Remove(name);
        }
        catch (KeyNotFoundException)
        {
            return;
        }
        
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
        foreach(var entry in ItemDict)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemController = obj.GetComponent<InventoryItemController>();
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemDescription = obj.transform.Find("ItemDescription").GetComponent<TMP_Text>();
            

            string path = "ScriptableObjects/Items/" + entry.Key;
            Item item = Resources.Load<Item>(path);

            itemController.item = item;
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemDescription.text = item.description; 
        }

    }

    /// It takes a string as a parameter, finds the item with the same name as the string, and then
    /// displays the item's name, icon, and description in the inventory description panel
    public void ShowItem(string toFind)
    {
        Destroy(InventoryDescriptionContent.GetChild(0).gameObject);

        string path = "ScriptableObjects/Items/" + toFind;
        Item item = Resources.Load<Item>(path);

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
        foreach(var entry in MailDict)
        {
            GameObject obj = Instantiate(InventoryMail, MailContent);
            var mailName = obj.transform.Find("MailName").GetComponent<TMP_Text>();
            var mailIcon = obj.transform.Find("MailIcon").GetComponent<Image>();
            var mailDescription = obj.transform.Find("MailDescription").GetComponent<TMP_Text>();

            string path = "ScriptableObjects/Mails/" + entry.Key;
            Mail mail = Resources.Load<Mail>(path);

            mailName.text = mail.mailName;
            mailIcon.sprite = mail.icon;
            mailDescription.text = mail.description;
        }
    }

      /// It takes a string as a parameter, finds the mail with the same name as the string, and then
      /// displays the mail's name, icon, and description
    public void ShowMail(string toFind)
    {
        Destroy(MailDescriptionContent.GetChild(0).gameObject);

        string path = "ScriptableObjects/Mails/" + toFind;
        Mail mail = Resources.Load<Mail>(path);

        GameObject obj1 = Instantiate(InventoryMailDescription, MailDescriptionContent);
        var mailName = obj1.transform.Find("MailName").GetComponent<TMP_Text>();
        var mailIcon = obj1.transform.Find("MailIcon").GetComponent<Image>();
        var mailDescription = obj1.transform.Find("MailDescription").GetComponent<TMP_Text>();

        mailName.text = mail.mailName;
        mailIcon.sprite = mail.icon;
        mailDescription.text = mail.description;
    }

    public void updateUltimateButtons(){
        foreach(KeyValuePair<int, bool> entry in UltimateDict){

            GameObject child = UltimatesContent.GetChild(entry.Key).gameObject;

            //TODO: change loaded sprite (unlocked)
            if(entry.Value){
                child.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/UltimateSprite"+entry.Key);
                child.GetComponent<Button>().onClick.RemoveListener(ButtonInactive);
            }
            else{
                child.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/skill not unlocked yet resized");
                child.GetComponent<Button>().onClick.AddListener(ButtonInactive);
            }
        }
    }
    void ButtonInactive(){}

    public void unlockUltimate(int ult){
        UltimateDict[ult] = true;
    }

    public void LoadData(GameData data)
    {
        ItemDict = data.ItemsDict;
        MailDict = data.MailDict;
        UltimateDict = data.UltimateDict;
    }
    
    public void SaveData(ref GameData data)
    {
        data.ItemsDict = (SerializableDictionary<string, int>)ItemDict;        
        data.MailDict = (SerializableDictionary<string, bool>)MailDict;
        data.UltimateDict = (SerializableDictionary<int, bool>)UltimateDict;
    }

}
