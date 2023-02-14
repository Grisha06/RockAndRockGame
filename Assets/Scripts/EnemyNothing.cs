using UnityEngine;

[AddComponentMenu("Enemies/Nothing")]
public class EnemyNothing : EnemyBaceAttakable 
{
    public override void NewFixedUpdate()
    {
        WingsPos.SetActive(false);
        if (attackIfRad && Vector2.Distance(pl.position, tr.position) < attackRadius)
            enemyBaceAction = EnemyBaceActions.Attack;
    }
}