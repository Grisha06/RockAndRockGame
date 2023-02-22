using UnityEngine;

[AddComponentMenu("Triggers/Inventory Item")]
public class InventoryTrigger : DestroyOnCollisionTrigger
{
    public InventoryItem Item;
}