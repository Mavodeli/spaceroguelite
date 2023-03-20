using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New QuestDescription",menuName = "Inventory/Create New QuestDescription")]

public class QuestDescription : ScriptableObject
{
    public int id;
    public string header;
    public string description;
    public Sprite icon;
    public float iconScale;
}

