using UnityEngine;

[AddComponentMenu("Triggers/Heal Trigger")]
public class HealTrigger : DestroyOnCollisionTrigger
{
    [Min(0)]
    public float Hp = 2;
    public override void Activate(NewEnemyBace entity)
    {
        if (entity.hp + Hp <= entity.maxHealth || entity.dynamicMaxHp)
        {
            base.Activate(entity);
            entity.Heal(Hp);
            base.Activate(entity);
        }
    }
}