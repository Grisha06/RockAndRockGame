using UnityEngine;

[AddComponentMenu("Triggers/Hand Hitter Trigger")]
public class HandHitter : MyTrigger
{
    public float damage = 1;
    public bool IsByHand = true;
    public override void Activate(Entity entity)
    {
        entity.AddDamage(damage, IsByHand);
    }
}
