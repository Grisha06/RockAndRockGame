using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Walker Jumper")]
public class EnemyWalkerJumper : EnemyBaceAttakable
{
    public GameObject rightWall;
    public GameObject leftWall;
    public GameObject rightWallUp;
    public GameObject leftWallUp;
    public BoxCollider2D WallsChecker;
    public float wallCheckRadius = 0.1f;
    public bool xDir = true;
    public float speed;
    public bool runIfRad = true;
    [SerializeField]
    private float goingUpMaxTime = 1.5f;
    private bool goingUp = false;
    private Vector2 goingUpPos;
    private float goingUpt=0f;
    public float runRadius;
    public float jumpForce;
    public override void NewStart()
    {
        StartCoroutine(AttackingEnumerator());
    }
    public override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
        WingsPos.SetActive(false);
        if (!goingUp)
        {
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
            CheckRightWallUp();
            CheckLeftWall();
            CheckLeftWallUp();
            if (!rightWall && leftWall && leftWallUp)
            {
                xDir = true;
            }
            if (rightWall && !leftWall && rightWallUp)
            {
                xDir = false;
            }

            if ((leftWall && !leftWallUp) || (rightWall && !rightWallUp))
            {
                Jump(jumpForce);
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
        else
        {
            if (rb.position.y >= goingUpPos.y - 0.5f || goingUpt>= goingUpMaxTime)
            {
                goingUp = false;
                goingUpt = 0f;
                goingUpPos = Vector2.zero;
                rb.velocity = new Vector2((xDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                goingUpt += Time.deltaTime;
                rb.position = Vector2.Lerp(rb.position, rb.position + new Vector2((xDir ? 1f : -1f) * speed * Time.deltaTime, Mathf.Sqrt(jumpForce) + WallsChecker.bounds.size.y), Time.deltaTime * 2);
            }
        }
    }
    public override void Jump(float jumpForce)
    {
        xDir = true;
        goingUpPos = rb.position + new Vector2(0, Mathf.Sqrt(jumpForce) + WallsChecker.bounds.size.y);
        goingUp = true;
        goingUpt = 0;
        an.Play("jump");
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
            rightWall = Physics2D.OverlapCircleAll(tr.position + new Vector3(WallsChecker.bounds.size.x / 2f + wallCheckRadius, WallsChecker.bounds.size.y / 2f), wallCheckRadius, groundLayer)[0].gameObject;
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
            leftWall = Physics2D.OverlapCircleAll(tr.position - new Vector3(WallsChecker.bounds.size.x / 2f + wallCheckRadius, -WallsChecker.bounds.size.y / 2f), wallCheckRadius, groundLayer)[0].gameObject;
        }
        catch
        {
            leftWall = null;
        }
    }
    public void CheckRightWallUp()
    {
        try
        {
            rightWallUp = Physics2D.OverlapCircleAll(tr.position + new Vector3(WallsChecker.bounds.size.x + wallCheckRadius, WallsChecker.bounds.size.y + Mathf.Sqrt(jumpForce)), wallCheckRadius, groundLayer)[0].gameObject;
        }
        catch
        {
            rightWallUp = null;
        }
    }
    public void CheckLeftWallUp()
    {
        try
        {
            leftWallUp = Physics2D.OverlapCircleAll(tr.position - new Vector3(WallsChecker.bounds.size.x + wallCheckRadius, -WallsChecker.bounds.size.y + Mathf.Sqrt(jumpForce)), wallCheckRadius, groundLayer)[0].gameObject;
        }
        catch
        {
            leftWallUp = null;
        }
    }

}
