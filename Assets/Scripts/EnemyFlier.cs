using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Traits;

[AddComponentMenu("Enemies/Flier")]
public class EnemyFlier : EnemyBaceAttakable, IEntityTrait<CanJump>
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

    protected override void NewAwake()
    {
        StartCoroutine(Flier());
    }
    protected override void NewFixedUpdate()
    {
        if (attackIfRad && Vector2.Distance(PlayerMover.single.tr.position, tr.position) < attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Attack;
        }
        if (Vector2.Distance(PlayerMover.single.tr.position, tr.position) >= runRadius && Vector2.Distance(PlayerMover.single.tr.position, tr.position) >= attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.None;
        }
        WingsPos.SetActive(true);
    }
    IEnumerator Flier()
    {
        while (true)
        {
            this.RandomJump(jumpForce,JumpTarget,rb,OnJumped,an);
            yield return new WaitForSeconds(jumpDest);
        }
    }
}
