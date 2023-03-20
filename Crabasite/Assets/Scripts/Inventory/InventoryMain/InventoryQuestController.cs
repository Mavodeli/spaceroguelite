using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryQuestController : MonoBehaviour
{
    public QuestDescription quest;

    public void Show()
    {
        InventoryManager.Instance.ShowQuest(quest.name);
    }
}
