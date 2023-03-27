using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Item",menuName = "Inventory/Create New Item")]

public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;
    public float iconScale;

    public string itemAmount;
}

