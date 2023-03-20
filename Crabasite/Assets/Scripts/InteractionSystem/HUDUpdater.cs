using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdater : MonoBehaviour
{
    private void Start()
    {
        DataPersistenceManager dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        ChangeSprite(dpm.getGameData().lastEquippedUltimate);
    }

    public void ChangeSprite(int ult)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/Inventory/UltimateSprite"+ult);
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }
}
