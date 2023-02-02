using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Walker_Rock : NewEnemyBace
{
    [HideInInspector]
    public EnemyBaceActions enemyBaceAction;
    [HideInInspector]
    public GameObject rightWall;
    [HideInInspector]
    public GameObject leftWall;
    public BoxCollider2D WallsChecker;
    public float wallCheckRadius = 0.1f;
    public bool xDir = true;
    public float speed;
    public bool runIfRad = true;
    public bool attackIfRad = true;
    public float runRadius;
    public float attackRadius;
    public MusicNoteSpavnerObj[] MusicNoteSpavner;
    public bool sameTimeAttack = false;
    public float attackTime;
    [HideInInspector]
    public int MusicNoteSpavnerSelNum = 0;
    public override void NewStart()
    {
        StartCoroutine(AttackingEnumerator());
    }
    public override void NewFixedUpdate()
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
    IEnumerator AttackingEnumerator()
    {
        while (true)
        {
            if (enemyBaceAction == EnemyBaceActions.Attack && hp > 0)
            {
                if (!sameTimeAttack)
                {
                    MusicNoteSpavnerSelNum = 0;
                    for (int i = 0; i < MusicNoteSpavner.Length; i++)
                    {
                        MusicNoteSpavnerSelNum = i;
                        yield return new WaitForSeconds(0.25f);
                        if (hp > 0)
                            an.Play("spawnMN");
                        yield return new WaitForSeconds(MusicNoteSpavner[MusicNoteSpavnerSelNum].SpavnTime);
                    }
                    MusicNoteSpavnerSelNum = 0;
                }
                else
                {
                    for (int i = 0; i < MusicNoteSpavner.Length && hp > 0; i++)
                    {
                        SpawnM(i);
                    }
                }
            }
            yield return new WaitForSeconds(attackTime);
        }
    }
    private void SpawnM(int mnssn)
    {
        GameObject mn = Instantiate(MusicNote, MusicNoteSpavner[mnssn].MusicNoteSpavner);
        mn.transform.rotation = Quaternion.identity;
        mn.transform.localScale = Vector3.one;
        mn.GetComponent<MusicNoteStart>().dir = MusicNoteSpavner[mnssn].MusicNoteSpavner;
        mn.GetComponent<MusicNoteStart>().force = MusicNoteSpavner[mnssn].force;
        mn.GetComponent<MusicNoteStart>().lifeTime = MusicNoteSpavner[mnssn].lifeTime;
        mn.GetComponent<MusicNoteStart>().damage = MusicNoteSpavner[mnssn].damage;
        mn.transform.SetParent(null);
    }
    private void SpawnMN()
    {
        SpawnM(MusicNoteSpavnerSelNum);
    }
}
