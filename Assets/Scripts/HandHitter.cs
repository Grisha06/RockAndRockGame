using UnityEngine;

[AddComponentMenu("Triggers/Hand Hitter Trigger")]
public class HandHitter : MyTrigger
{
    public float damage = 1;
    public Entity.DamageType damageType = Entity.DamageType.Hand;
    public override void Activate(Entity entity)
    {
        entity.AddDamage(damage, damageType);
    }
}

