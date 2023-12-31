using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContent : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D itemCollider;

    public void Initialize(MapContentData itemData, Vector2? placementPos)
    {
        //set sprite
        spriteRenderer.sprite = itemData.sprite;
        //set sprite offset
        spriteRenderer.transform.localPosition = (Vector2)placementPos + new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
        itemCollider.size = itemData.size;
        itemCollider.offset = new Vector2(0, 0);
    }
}

