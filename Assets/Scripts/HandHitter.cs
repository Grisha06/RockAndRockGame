using UnityEngine;

[AddComponentMenu("Triggers/Hand Hitter Trigger")]
public class HandHitter : MyTrigger
{
    public int damage = 1;
    public bool IsByHand = true;
    public override void Activate(NewEnemyBace entity)
    {
        entity.AddDamage(damage, IsByHand);
    }
}
