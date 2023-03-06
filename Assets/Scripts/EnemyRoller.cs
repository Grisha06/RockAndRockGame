using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Traits;

[AddComponentMenu("Enemies/Roller")]
public class EnemyRoller : EnemyBaceAttakable, IEntityTrait<CanWalk>, IEntityTrait<CanUpdateActions>, IEntityTrait<CanCheckWalls>
{
    public GameObject rightWall;
    public GameObject leftWall;
    public CircleCollider2D WallsChecker;
    public float wallCheckRadius = 0.1f;
    public bool xDir = true;
    public float speed;
    public bool runIfRad = true;
    public float runRadius;

    protected override void NewFixedUpdate()
    {
        this.WalkUpdate(ref xDir, rb, ref enemyBaceAction, runIfRad, attackIfRad, () => { this.CheckRightWall(ref rightWall, WallsChecker, tr, wallCheckRadius, groundLayer); }, () => { this.CheckLeftWall(ref leftWall, WallsChecker, tr, wallCheckRadius, groundLayer); }, rightWall, leftWall, tr, runRadius, attackRadius);

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
        }
        if (PlayerMover.single.tr.position.x != tr.position.x)
        {
            this.Roll(speed, xDir, rb);
        }
    }
}
