using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Walker Hider")]
public class EnemyWalkerHider : Enemy_Walker_Rock
{
    public BoxCollider2D hidedColl;
    public Sprite hiddedSprite;
    public Sprite unhiddedSprite;
    protected override void NewFixedUpdate()
    {
        WingsPos.SetActive(false);
        if (runIfRad && Vector2.Distance(PlayerMover.single.tr.position, tr.position) < runRadius && Vector2.Distance(PlayerMover.single.tr.position, tr.position) >= attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Run;
        }
        if (attackIfRad && Vector2.Distance(PlayerMover.single.tr.position, tr.position) < attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Attack;
        }
        if (Vector2.Distance(PlayerMover.single.tr.position, tr.position) >= runRadius && Vector2.Distance(PlayerMover.single.tr.position, tr.position) >= attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.None;
        }
        CheckRightWall();
        CheckLeftWall();
        if (!rightWall && leftWall)
        {
            xDir = true;
        }
        if (rightWall && !leftWall)
        {
            xDir = false;
        }

        if (enemyBaceAction == EnemyBaceActions.Run || enemyBaceAction == EnemyBaceActions.Attack)
        {
            sr.sprite = hiddedSprite;
            hidedColl.enabled = true;
            WallsChecker.enabled = false;
        }
        else
        {
            sr.sprite = unhiddedSprite;
            hidedColl.enabled = false;
            WallsChecker.enabled = true;
            rb.velocity = new Vector2((xDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);
        }
    }
}
