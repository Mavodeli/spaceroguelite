using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Mail",menuName = "Mail/Create New Mail")]

public class Mail : ScriptableObject
{
    public int id;
    public string mailName;
    public string description;
    public Sprite icon;
}

