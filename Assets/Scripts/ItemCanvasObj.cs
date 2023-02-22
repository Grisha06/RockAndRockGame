using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCanvasObj : MonoBehaviour
{
    [SerializeField]
    private Image image;
    public void Activate(InventoryItem inventoryItem)
    {
        image.sprite = inventoryItem.sprite;
    }
}
