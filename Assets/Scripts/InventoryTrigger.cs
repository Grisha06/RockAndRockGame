using UnityEngine;

[AddComponentMenu("Triggers/Inventory Item")]
public class InventoryTrigger : MyTrigger
{
    public override void Activate(NewEnemyBace entity)
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}