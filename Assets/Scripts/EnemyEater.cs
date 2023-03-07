using UnityEngine;

[AddComponentMenu("Enemies/Eater")]
public class EnemyEater : Entity
{
    public bool isAttackable = true;
    public float attackRadius = 2f;

    protected override void DoFlip()
    {
        Flip(PlayerMover.single.tr.position.x < tr.position.x);
    }
    protected override void NewFixedUpdate()
    {
        WingsPos.SetActive(false);
        if (isAttackable && Vector2.Distance(PlayerMover.single.tr.position, tr.position) < attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Attack;
        }
        else
        {
            enemyBaceAction = EnemyBaceActions.None;
        }

        an.SetBool("attacking", enemyBaceAction == EnemyBaceActions.Attack);
    }
}
