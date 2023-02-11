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
        //TODO: only on newGame, else last equipped
        ChangeSprite(3);
        UltimateSprite.size *= .26f; //0.26f
        UltimateSprite.size *= .26f;
    }

    public void ChangeSprite(int ult)
    {
        UltimateSprite.sprite = Resources.Load<Sprite>("Sprites/Inventory/UltimateSprite"+ult);
        
    }
    
}
