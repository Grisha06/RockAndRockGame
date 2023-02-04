using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Walker")]
public class Enemy_Walker_Rock : EnemyBaceAttakable
{
    [HideInInspector]
    public GameObject rightWall;
    [HideInInspector]
    public GameObject leftWall;
    public BoxCollider2D WallsChecker;
    public float wallCheckRadius = 0.1f;
    public bool xDir = true;
    public float speed;
    public bool runIfRad = true;
    public float runRadius;
    public override void NewStart()
    {
        StartCoroutine(AttackingEnumerator());
    }
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
        rb.velocity = new Vector2((xDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);

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
                rb.velocity = new Vector2((xDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);
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
            rightWall = Physics2D.OverlapCircleAll(tr.position + new Vector3(WallsChecker.size.x / 2f + wallCheckRadius, WallsChecker.size.y / 2f), wallCheckRadius, groundLayer)[0].gameObject;
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
            leftWall = Physics2D.OverlapCircleAll(tr.position - new Vector3(WallsChecker.size.x / 2f + wallCheckRadius, -WallsChecker.size.y / 2f), wallCheckRadius, groundLayer)[0].gameObject;
        }
        catch
        {
            leftWall = null;
        }
    }
    
}
