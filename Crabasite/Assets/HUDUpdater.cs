using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdater : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer UltimateSprite;

    [SerializeField]
    private List<Transform> UltimateSprites;

    private void Start()
    {
        ChangeSprite(1);   
    }

    public void ChangeSprite(int ult)
    {
        UltimateSprite.sprite = UltimateSprites[ult].GetComponent<Image>().sprite;
    }
    
}
