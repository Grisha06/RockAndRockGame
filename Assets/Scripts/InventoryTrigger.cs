using UnityEngine;

[AddComponentMenu("Triggers/Inventory Item")]
public class InventoryTrigger : DestroyOnCollisionTrigger
{
    public InventoryItem Item;
    public override void Activate(Entity entity)
    {
        entity.OnInvTriggerEntered.Invoke(this);
        base.Activate(entity);
    }
}