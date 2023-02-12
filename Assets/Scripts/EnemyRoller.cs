using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Roller")]
public class EnemyRoller : EnemyBaceAttakable
{
    [HideInInspector]
    public GameObject rightWall;
    [HideInInspector]
    public GameObject leftWall;
    public CircleCollider2D WallsChecker;
    public float wallCheckRadius = 0.1f;
    public bool xDir = true;
    public float speed;
    public bool runIfRad = true;
    public float runRadius;

    public override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
        WingsPos.SetActive(false);
        if (runIfRad && Vector2.Distance(pl.position, tr.position) < runRadius && Vector2.Distance(pl.position, tr.position) >= attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Run;
        }
        if (attackIfRad && Vector2.Distance(pl.position, tr.position) < attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Attack;
        }
        if (Vector2.Distance(pl.position, tr.position) >= runRadius && Vector2.Distance(pl.position, tr.position) >= attackRadius)
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
        rb.angularVelocity = (xDir ? -1f : 1f) * speed * Time.deltaTime;

        if (enemyBaceAction == EnemyBaceActions.Run)
        {
            if (pl.position.x > tr.position.x)
            {
                xDir = true;
            }
            if (pl.position.x < tr.position.x)
            {
                xDir = false;
            }
            if (pl.position.x != tr.position.x)
            {
                rb.angularVelocity = (xDir ? -1f : 1f) * speed * Time.deltaTime;
            }
        }
    }
    public override void NewUpdate()
    {
        base.NewUpdate();
    }
    public override void NewOnCollisionEnter2D(Collision2D collision)
    {

    }
    public void CheckRightWall()
    {
        try
        {
            rightWall = Physics2D.OverlapCircleAll(tr.position + new Vector3(WallsChecker.radius + wallCheckRadius, WallsChecker.radius), wallCheckRadius, groundLayer)[0].gameObject;
        }
        catch
        {
            rightWall = null;
        }
    }
    public void CheckLeftWall()
    {
        try
        {
            leftWall = Physics2D.OverlapCircleAll(tr.position - new Vector3(WallsChecker.radius + wallCheckRadius, -WallsChecker.radius), wallCheckRadius, groundLayer)[0].gameObject;
            Debug.DrawRay(tr.position - new Vector3(WallsChecker.radius + wallCheckRadius, -WallsChecker.radius), Vector3.right * wallCheckRadius, Color.green);
        }
        catch
        {
            leftWall = null;
        }
    }
}
