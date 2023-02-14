﻿using UnityEngine;

[AddComponentMenu("Triggers/Heal Trigger")]
public class HealTrigger : DestroyOnCollisionTrigger
{
    [Min(0)]
    public float Hp = 2;
    public override void Activate(NewEnemyBace entity)
    {
        base.Activate(entity);
        entity.hp += Hp;
        base.Activate(entity);
    }
}