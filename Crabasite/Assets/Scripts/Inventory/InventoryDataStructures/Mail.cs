using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Mail",menuName = "Inventory/Create New Mail")]

/// Mail is a ScriptableObject that contains an id, a name, a description, and an icon
public class Mail : ScriptableObject
{
    public int id;
    public string mailName;
    public string description;
    public Sprite icon;
}

