using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Plant")]
public class EnemyPlant : EnemyBaceAttakable
{
    public override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
        WingsPos.SetActive(false);
        if (attackIfRad && Vector2.Distance(pl.position, tr.position) < attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Attack;
        }
    }
}
