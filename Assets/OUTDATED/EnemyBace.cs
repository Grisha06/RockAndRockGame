using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace OUTDATED_Traits
{
    abstract class EntityTrait { }

    interface IEntityTrait<T> where T : EntityTrait { }

    class CanUpdateActions : EntityTrait { }

    class CanWalk : EntityTrait { }

    class CanAttack : EntityTrait { }

    class CanJump : EntityTrait { }

    class CanCheckWalls : EntityTrait { }

    class CanCheckLeftWall : EntityTrait { }

    class Penguin : IEntityTrait<CanWalk> { }

    static class EntityTraits
    {
        public static void Walk(this IEntityTrait<CanWalk> trait, float speed, bool xDir, Rigidbody2D rb, bool FlipxDir = false)
        {
            rb.velocity = new Vector2((xDir ? 1f : -1f) * (!FlipxDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);
        }
        public static void Roll(this IEntityTrait<CanWalk> trait, float speed, bool xDir, Rigidbody2D rb)
        {
            rb.angularVelocity = (xDir ? -1f : 1f) * speed * Time.deltaTime;
        }
        public static void RandomJump(this IEntityTrait<CanJump> trait, float jumpForce, Transform JumpTarget, Rigidbody2D rb, UnityEvent OnJumped = null, Animator an = null)
        {
            JumpTarget.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(45, 135));
            rb.AddForce(JumpTarget.right * jumpForce, ForceMode2D.Impulse);
            OnJumped?.Invoke();
            an?.Play("jump");
        }
        public static void Climb(this IEntityTrait<CanJump> trait, float jumpForce, Rigidbody2D rb, ref bool xDir, ref Vector2 goingUpPos, ref bool goingUp, ref float goingUpt, Collider2D WallsChecker, UnityEvent OnJumped = null, Animator an = null)
        {
            xDir = true;
            goingUpPos = rb.position + new Vector2(0, Mathf.Sqrt(jumpForce) + WallsChecker.bounds.size.y);
            goingUp = true;
            goingUpt = 0;
            OnJumped?.Invoke();
            an?.Play("jump");
        }
        public static void PlayerJump(this IEntityTrait<CanJump> trait, float jumpForce, Rigidbody2D rb, Transform arrowObj, UnityEvent OnJumped = null, Animator an = null)
        {
            rb.AddForce(arrowObj.forward * jumpForce, ForceMode2D.Impulse);
            OnJumped?.Invoke();
            an?.Play("jump");
        }
        public static void WalkUpdate(
            this IEntityTrait<CanUpdateActions> trait,
            ref bool xDir,
            Rigidbody2D rb,
            ref EnemyBaceActions enemyBaceAction,
            bool runIfRad,
            bool attackIfRad,
            Action CheckRightWall,
            Action CheckLeftWall,
            GameObject rightWall,
            GameObject leftWall,
            Transform tr,
            float runRadius,
            float attackRadius
            )
        {
            if (runIfRad && Vector2.Distance(PlayerMover.single.tr.position, tr.position) < runRadius && Vector2.Distance(PlayerMover.single.tr.position, tr.position) >= attackRadius)
            {
                enemyBaceAction = EnemyBaceActions.Run;
            }
            if (attackIfRad && Vector2.Distance(PlayerMover.single.tr.position, tr.position) < attackRadius)
            {
                enemyBaceAction = EnemyBaceActions.Attack;
            }
            if (Vector2.Distance(PlayerMover.single.tr.position, tr.position) >= runRadius && Vector2.Distance(PlayerMover.single.tr.position, tr.position) >= attackRadius)
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
        }
        public static void CheckRightWall(this IEntityTrait<CanCheckWalls> trait, ref GameObject rightWall, Collider2D WallsChecker, Transform tr, float wallCheckRadius, LayerMask groundLayer)
        {
            try
            {
                rightWall = Physics2D.OverlapCircleAll(tr.position + new Vector3(WallsChecker.bounds.size.x / 2f + wallCheckRadius, WallsChecker.bounds.size.y / 2f + wallCheckRadius), wallCheckRadius, groundLayer)[0].gameObject;
            }
            catch
            {
                rightWall = null;
            }
        }
        public static void CheckLeftWall(this IEntityTrait<CanCheckWalls> trait, ref GameObject leftWall, Collider2D WallsChecker, Transform tr, float wallCheckRadius, LayerMask groundLayer)
        {
            try
            {
                leftWall = Physics2D.OverlapCircleAll(tr.position - new Vector3(WallsChecker.bounds.size.x / 2f + wallCheckRadius, -WallsChecker.bounds.size.y / 2f + wallCheckRadius), wallCheckRadius, groundLayer)[0].gameObject;
            }
            catch
            {
                leftWall = null;
            }
        }
        public static void CheckRightWallUp(this IEntityTrait<CanCheckWalls> trait, ref GameObject rightWallUp, Collider2D WallsChecker, Transform tr, float wallCheckRadius, LayerMask groundLayer)
        {
            try
            {
                rightWallUp = Physics2D.OverlapCircleAll(tr.position + new Vector3(WallsChecker.bounds.size.x + wallCheckRadius, WallsChecker.bounds.size.y * 2f), wallCheckRadius, groundLayer)[0].gameObject;
            }
            catch
            {
                rightWallUp = null;
            }
        }
        public static void CheckLeftWallUp(this IEntityTrait<CanCheckWalls> trait, ref GameObject leftWallUp, Collider2D WallsChecker, Transform tr, float wallCheckRadius, LayerMask groundLayer)
        {
            try
            {
                leftWallUp = Physics2D.OverlapCircleAll(tr.position - new Vector3(WallsChecker.bounds.size.x + wallCheckRadius, -WallsChecker.bounds.size.y * 2f), wallCheckRadius, groundLayer)[0].gameObject;
            }
            catch
            {
                leftWallUp = null;
            }
        }
    }
}

namespace OUTDATED
{
    public enum EnemyBaceBehaviors
    {
        Fly,
        Walk,
        Stand,
        Roll
    }

    [AddComponentMenu("Enemies/Enemy OUTDATED")]
    public class EnemyBace : MonoBehaviour
    {
        public float hp = 10;
        public int musicArm = 0;
        public int handArm = 5;
        public EnemyBaceBehaviors enemyBaceBehavior;
        public EnemyBaceActions enemyBaceAction;
        public bool runIfRad = true;
        public bool attackIfRad = true;
        public float runRadius;
        public float attackRadius;
        public float attackTime;
        public bool sameTimeAttack = false;
        public BaseMusicNoteSpavnerObj[] MusicNoteSpavner;
        public DropObj[] Drop;
        public int MusicNoteSpavnerSelNum = 0;
        public GameObject MusicNote;
        public bool xDir = true;
        public float speed;
        public float jumpForce;
        public float jumpDest;
        public float wallCheckRadius = 0.1f;
        public GameObject rightWall;
        public GameObject leftWall;
        public Transform JumpTarget;
        public BoxCollider2D WallsChecker;
        public CircleCollider2D WallsCheckerRoll;
        public Transform pl;
        public GameObject WingsPos;
        public LayerMask groundLayer;
        public Animator an;
        Transform tr;
        Rigidbody2D rb;
        private void Start()
        {
            tr = transform;
            pl = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            StartCoroutine(Flier());
            StartCoroutine(AttackC());
        }
        public void CheckRightWall()
        {
            if (enemyBaceBehavior == EnemyBaceBehaviors.Walk)
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
            if (enemyBaceBehavior == EnemyBaceBehaviors.Roll)
            {
                try
                {
                    rightWall = Physics2D.OverlapCircleAll(tr.position + new Vector3(WallsCheckerRoll.radius + wallCheckRadius, WallsCheckerRoll.radius), wallCheckRadius, groundLayer)[0].gameObject;
                }
                catch
                {
                    rightWall = null;
                }
            }
        }
        public void CheckLeftWall()
        {
            if (enemyBaceBehavior == EnemyBaceBehaviors.Walk)
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
            if (enemyBaceBehavior == EnemyBaceBehaviors.Roll)
            {
                try
                {
                    leftWall = Physics2D.OverlapCircleAll(tr.position - new Vector3(WallsCheckerRoll.radius + wallCheckRadius, -WallsCheckerRoll.radius), wallCheckRadius, groundLayer)[0].gameObject;
                    Debug.DrawRay(tr.position - new Vector3(WallsCheckerRoll.radius + wallCheckRadius, -WallsCheckerRoll.radius), Vector3.right * wallCheckRadius, Color.green);
                }
                catch
                {
                    leftWall = null;
                }
            }
        }
        private void FixedUpdate()
        {
            if (hp > 0)
            {
                if (runIfRad && Vector2.Distance(pl.position, tr.position) < runRadius && Vector2.Distance(pl.position, tr.position) >= attackRadius && enemyBaceBehavior != EnemyBaceBehaviors.Fly)
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

                if (enemyBaceBehavior == EnemyBaceBehaviors.Walk || enemyBaceBehavior == EnemyBaceBehaviors.Roll)
                {
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
                    if (enemyBaceBehavior == EnemyBaceBehaviors.Walk)
                    {
                        rb.velocity = new Vector2((xDir ? 1f : -1f) * speed * Time.deltaTime, rb.velocity.y);
                    }
                    if (enemyBaceBehavior == EnemyBaceBehaviors.Roll)
                    {
                        rb.angularVelocity = (xDir ? -1f : 1f) * speed * Time.deltaTime;
                    }
                    WingsPos.SetActive(false);
                }
                if (enemyBaceBehavior == EnemyBaceBehaviors.Fly)
                {
                    WingsPos.SetActive(true);
                }
                if (enemyBaceBehavior == EnemyBaceBehaviors.Stand)
                {
                    WingsPos.SetActive(false);
                }
                if (enemyBaceAction == EnemyBaceActions.Run)
                {
                    if (enemyBaceBehavior == EnemyBaceBehaviors.Walk || enemyBaceBehavior == EnemyBaceBehaviors.Stand)
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
                    if (enemyBaceBehavior == EnemyBaceBehaviors.Roll)
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
                            rb.angularVelocity = (xDir ? -1f : 1f) * speed * Time.deltaTime;
                        }
                    }
                }
            }
        }
        private void LateUpdate()
        {
            if (hp <= 0 && hp > -100)
            {
                an.Play("die");
                hp = -101;
            }
        }
        public void SelfDestroy()
        {
            GameObject k;
            if (Drop.Length != 0)
            {
                for (int j = 0; j < Drop.Length; j++)
                {
                    if (Drop[j].chance >= Random.Range(0f, 100f))
                    {
                        for (int i = 0; i < Drop[j].number; i++)
                        {
                            k = Instantiate(Drop[j].obj, transform);
                            k.transform.localPosition = Vector3.zero;
                            k.transform.SetParent(null);
                        }
                    }
                }
            }
            Destroy(gameObject);
        }
        private void Jump()
        {
            JumpTarget.rotation = Quaternion.Euler(0, 0, Random.Range(45, 135));
            rb.AddForce(JumpTarget.right * jumpForce, ForceMode2D.Impulse);
            an.Play("jump");
        }

        private void SpawnM(int mnssn)
        {
            GameObject mn = Instantiate(MusicNote, MusicNoteSpavner[mnssn].MusicNoteSpavner);
            mn.transform.rotation = Quaternion.identity;
            mn.transform.localScale = Vector3.one;
            mn.GetComponent<MusicNoteStart>().dir = MusicNoteSpavner[mnssn].MusicNoteSpavner;
            mn.GetComponent<MusicNoteStart>().force = MusicNoteSpavner[mnssn].Force;
            mn.GetComponent<MusicNoteStart>().lifeTime = MusicNoteSpavner[mnssn].Lifetime;
            mn.GetComponent<MusicNoteStart>().damage = MusicNoteSpavner[mnssn].Damage;
            mn.transform.SetParent(null);
        }
        private void SpawnMN()
        {
            SpawnM(MusicNoteSpavnerSelNum);
        }
        IEnumerator Flier()
        {
            while (true)
            {
                if (enemyBaceBehavior == EnemyBaceBehaviors.Fly && hp > 0)
                {
                    Jump();
                }
                yield return new WaitForSeconds(jumpDest);
            }
        }
        IEnumerator AttackC()
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
                            yield return new WaitForSeconds(MusicNoteSpavner[MusicNoteSpavnerSelNum].SpawnTime);
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (hp > 0)
            {
                if (collision.gameObject.CompareTag("notePL"))
                {
                    hp -= musicArm < collision.gameObject.GetComponent<MusicNoteStart>().damage ? collision.gameObject.GetComponent<MusicNoteStart>().damage - musicArm : 1;
                    an.Play("damage");
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
