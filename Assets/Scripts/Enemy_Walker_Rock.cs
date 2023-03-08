using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Walker")]
public class Enemy_Walker_Rock : EntityAttakable
{
    protected GameObject rightWall;
    protected GameObject leftWall;
    [SerializeField] protected BoxCollider2D WallsChecker;
    [SerializeField] protected float wallCheckRadius = 0.1f;
    [SerializeField] protected bool xDir = true;
    [SerializeField] protected bool FlipxDir = false;
    [SerializeField] protected float speed;
    [SerializeField] protected bool runIfRad = true;
    [SerializeField] protected float runRadius;

    protected override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
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
        rb.velocity = new Vector2((xDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);

        if (enemyBaceAction == EnemyBaceActions.Run)
        {
            if (PlayerMover.single.tr.position.x > tr.position.x)
            {
                xDir = true;
            }
            if (PlayerMover.single.tr.position.x < tr.position.x)
            {
                xDir = false;
            }
            if (PlayerMover.single.tr.position.x != tr.position.x)
            {
                rb.velocity = new Vector2((xDir ? 1f : -1f) * (!FlipxDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);
            }
        }
    }
    protected void CheckRightWall()
    {
        try
        {
            rightWall = Physics2D.OverlapCircleAll(tr.position + new Vector3(WallsChecker.size.x / 2f + wallCheckRadius, WallsChecker.size.y / 2f), wallCheckRadius, groundLayer)[0].gameObject;
        }
        catch
        {
            rightWall = null;
        }
    }
    protected void CheckLeftWall()
    {
        try
        {
            leftWall = Physics2D.OverlapCircleAll(tr.position - new Vector3(WallsChecker.size.x / 2f + wallCheckRadius, -WallsChecker.size.y / 2f), wallCheckRadius, groundLayer)[0].gameObject;
        }
        catch
        {
            leftWall = null;
        }
    }
}
