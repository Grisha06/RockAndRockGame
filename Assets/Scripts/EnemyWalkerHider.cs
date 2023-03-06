using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Traits;

[AddComponentMenu("Enemies/Walker Hider")]
public class EnemyWalkerHider : EnemyBaceAttakable, IEntityTrait<CanWalk>, IEntityTrait<CanUpdateActions>, IEntityTrait<CanCheckWalls>
{
    public GameObject rightWall;
    public GameObject leftWall;
    public BoxCollider2D WallsChecker;
    public float wallCheckRadius = 0.1f;
    public bool xDir = true;
    public bool FlipxDir = false;
    public float speed;
    public bool runIfRad = true;
    public float runRadius;
    public BoxCollider2D hidedColl;
    public Sprite hiddedSprite;
    public Sprite unhiddedSprite;
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
            this.Walk(speed, xDir, rb, FlipxDir);
        }
    }
}
