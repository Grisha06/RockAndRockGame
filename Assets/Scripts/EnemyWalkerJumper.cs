using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Traits;

[AddComponentMenu("Enemies/Walker Jumper")]
public class EnemyWalkerJumper : EnemyBaceAttakable, IEntityTrait<CanJump>, IEntityTrait<CanWalk>, IEntityTrait<CanUpdateActions>, IEntityTrait<CanCheckWalls>
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
    public GameObject rightWallUp;
    public GameObject leftWallUp;
    [SerializeField]
    private float goingUpMaxTime = 1.5f;
    public bool goingUp = false;
    public Vector2 goingUpPos;
    public float goingUpt = 0f;
    public float jumpForce;

    protected override void NewFixedUpdate()
    {
        if (!goingUp)
        {
            this.WalkUpdate(ref xDir, rb, ref enemyBaceAction, runIfRad, attackIfRad, () => { this.CheckRightWall(ref rightWall, WallsChecker, tr, wallCheckRadius, groundLayer); }, () => { this.CheckLeftWall(ref leftWall, WallsChecker, tr, wallCheckRadius, groundLayer); }, rightWall, leftWall, tr, runRadius, attackRadius);
            this.Walk(speed, xDir, rb, FlipxDir);
            this.CheckRightWallUp(ref rightWallUp, WallsChecker, tr, wallCheckRadius, groundLayer);
            this.CheckLeftWallUp(ref leftWallUp, WallsChecker, tr, wallCheckRadius, groundLayer);

            if ((leftWall && !leftWallUp) || (rightWall && !rightWallUp))
            {
                this.Climb(jumpForce, rb, ref xDir, ref goingUpPos, ref goingUp, ref goingUpt, WallsChecker, an: an);
            }
        }
        else
        {
            if (rb.position.y >= goingUpPos.y - 0.5f || goingUpt >= goingUpMaxTime)
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
