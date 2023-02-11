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
    }

    public void ChangeSprite(int ult)
    {   
        Sprite sprite = Resources.Load<Sprite>("Sprites/Inventory/UltimateSprite"+ult);
        UltimateSprite.sprite = sprite;
        UltimateSprite.size = new Vector2(sprite.bounds.extents.x, sprite.bounds.extents.y)*.13f;
    }
    
}
