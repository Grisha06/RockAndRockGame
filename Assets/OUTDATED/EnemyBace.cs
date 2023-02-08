using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBaceBehaviors
{
    Fly,
    Walk,
    Stand,
    Roll
}
public enum EnemyBaceActions
{
    Run,
    Attack,
    None
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
    public MusicNoteSpavnerObj[] MusicNoteSpavner;
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

