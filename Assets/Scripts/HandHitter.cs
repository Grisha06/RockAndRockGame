using UnityEngine;

[AddComponentMenu("Triggers/Hand Hitter Trigger")]
public class HandHitter : MyTrigger
{
    public int damage = 1;
    public bool IsByHand = true;
    public override void Activate(NewEnemyBace entity)
    {
        base.Activate(entity);
        entity.AddDamage(damage, IsByHand);
    }
}
