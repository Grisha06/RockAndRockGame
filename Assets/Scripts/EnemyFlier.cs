using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Flier")]
public class EnemyFlier : EnemyBaceAttakable
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
    public float jumpForce;
    [Min(0.5f)]
    public float jumpDest;
    public override void NewStart()
    {
        StartCoroutine(AttackingEnumerator());
        StartCoroutine(Flier());
    }
    public override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
        if (attackIfRad && Vector2.Distance(pl.position, tr.position) < attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Attack;
        }
        if (Vector2.Distance(pl.position, tr.position) >= runRadius && Vector2.Distance(pl.position, tr.position) >= attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.None;
        }
        WingsPos.SetActive(true);
    }
    public override void NewUpdate()
    {
        base.NewUpdate();
    }
    public override void NewOnCollisionEnter2D(Collision2D collision)
    {

    }
    IEnumerator Flier()
    {
        while (true)
        {
            Jump(jumpForce);
            yield return new WaitForSeconds(jumpDest);
        }
    }
}
