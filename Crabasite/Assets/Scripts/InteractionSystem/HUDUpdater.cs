using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdater : MonoBehaviour
{
    private Image UltimateSprite;

    private void Start()
    {
        UltimateSprite = transform.GetChild(0).GetComponent<Image>();

        DataPersistenceManager dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        ChangeSprite(dpm.getGameData().lastEquippedUltimate);
    }

    public void ChangeSprite(int ult)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/Inventory/UltimateSprite"+ult);
        UltimateSprite.sprite = sprite;
    }
}
