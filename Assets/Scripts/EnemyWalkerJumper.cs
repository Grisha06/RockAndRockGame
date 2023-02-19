using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Walker Jumper")]
public class EnemyWalkerJumper : Enemy_Walker_Rock
{
    public GameObject rightWallUp;
    public GameObject leftWallUp;
    [SerializeField]
    private float goingUpMaxTime = 1.5f;
    private bool goingUp = false;
    private Vector2 goingUpPos;
    private float goingUpt=0f;
    public float jumpForce;

    protected override void NewFixedUpdate()
    {
        WingsPos.SetActive(false);
        if (!goingUp)
        {
            base.NewFixedUpdate();
            CheckRightWallUp();
            CheckLeftWallUp();
            if ((leftWall && !leftWallUp) || (rightWall && !rightWallUp))
            {
                Jump(jumpForce);
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
