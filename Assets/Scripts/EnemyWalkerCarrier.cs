using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Traits;

[AddComponentMenu("Enemies/Walker Carrier")]
public class EnemyWalkerCarrier : EnemyBaceAttakable, IEntityTrait<CanWalk>, IEntityTrait<CanUpdateActions>, IEntityTrait<CanCheckWalls>
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
    [SerializeField]
    protected Entity CarringGameObject;
    [SerializeField]
    protected Transform ToCarringTransform;
    private Entity cgo;
    private RigidbodyConstraints2D cgoConstraints;
    protected override void NewAwake()
    {
        base.NewAwake();
        if (CarringGameObject)
        {
            cgo = Instantiate(CarringGameObject);
            cgo.tr.SetParent(ToCarringTransform);
            cgo.tr.localPosition = Vector3.zero;
            cgoConstraints = cgo.rb.constraints;
            cgo.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    protected override void NewFixedUpdate()
    {
        this.WalkUpdate(ref xDir, rb, ref enemyBaceAction, runIfRad, attackIfRad, () => { this.CheckRightWall(ref rightWall, WallsChecker, tr, wallCheckRadius, groundLayer); }, () => { this.CheckLeftWall(ref leftWall, WallsChecker, tr, wallCheckRadius, groundLayer); }, rightWall, leftWall, tr, runRadius, attackRadius);
        this.Walk(speed, xDir, rb, FlipxDir);

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
            if (PlayerMover.single.tr.position.x != tr.position.x)
            {
                rb.velocity = new Vector2((xDir ? 1f : -1f) * (!FlipxDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);
            }
        }
        if (cgo) cgo.tr.position = ToCarringTransform.position;
    }
    public override void SelfDestroy()
    {
        if (cgo)
        {
            cgo.tr.SetParent(null);
            cgo.tr.position = ToCarringTransform.position;
            cgo.rb.constraints = cgoConstraints;
        }
        base.SelfDestroy();
    }
}
