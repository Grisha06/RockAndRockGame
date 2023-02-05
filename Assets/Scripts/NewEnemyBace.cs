using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class EnemyBaceAttakable : NewEnemyBace
{
    public float attackRadius;
    public BaseMusicNoteSpavnerObj[] MusicNoteSpavner;
    public bool sameTimeAttack = false;
    public float attackTime;
    [HideInInspector]
    public int MusicNoteSpavnerSelNum = 0;
    public bool attackIfRad = true;
    public override void NewUpdate() { }
    public override void NewOnCollisionEnter2D(Collision2D collision) { }
    public override void NewFixedUpdate() { }
    public override void NewOnTriggerStay2D(Collider2D collision) { }
    public override void NewOnTriggerEnter2D(Collider2D collision) { }
    public void SpawnM(int mnssn)
    {
        GameObject mn = Instantiate(MusicNote, MusicNoteSpavner[mnssn].MusicNoteSpavner);
        MusicNoteSpavner[mnssn].Attack(mn);
        mn.transform.rotation = Quaternion.identity;
        mn.transform.localScale = Vector3.one;
        mn.GetComponent<MusicNoteStart>().dir = MusicNoteSpavner[mnssn].MusicNoteSpavner;
        mn.GetComponent<MusicNoteStart>().force = MusicNoteSpavner[mnssn].Force;
        mn.GetComponent<MusicNoteStart>().lifeTime = MusicNoteSpavner[mnssn].Lifetime;
        mn.GetComponent<MusicNoteStart>().damage = MusicNoteSpavner[mnssn].Damage;
        mn.transform.SetParent(null);
    }
    public void SpawnMN()
    {
        SpawnM(MusicNoteSpavnerSelNum);
    }
    public virtual IEnumerator AttackingEnumerator()
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
}

public abstract class NewEnemyBace : MonoBehaviour, IDamagable
{
    [Header("Inheritanced fields")]
    public EnemyBaceActions enemyBaceAction;
    public int hp;
    public int musicArm;
    public int handArm;
    public DropObj[] Drop;
    public GameObject MusicNote;
    public Transform JumpTarget;
    public GameObject WingsPos;
    public LayerMask groundLayer;
    [HideInInspector]
    public Transform pl;
    public Animator an;
    [HideInInspector]
    public Transform tr;
    [HideInInspector]
    public Rigidbody2D rb;
    [Header("Custom fields")]
    public bool dont_totch_me;

    private void Start()
    {
        tr = transform;
        pl = GameObject.FindGameObjectWithTag("Player").transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        an = gameObject.GetComponent<Animator>();
        NewStart();
    }
    public abstract void NewStart();
    public abstract void NewFixedUpdate();
    public abstract void NewUpdate();
    public abstract void NewOnCollisionEnter2D(Collision2D collision);
    public abstract void NewOnTriggerStay2D(Collider2D collision);
    public abstract void NewOnTriggerEnter2D(Collider2D collision);

    private void FixedUpdate()
    {
        if (hp > 0)
        {
            NewFixedUpdate();
        }
    }
    private void Update()
    {
        if (hp > 0)
        {
            NewUpdate();
        }
    }
    private void LateUpdate()
    {
        if (hp <= 0 && hp > -100)
        {
            an.Play("die");
            an.SetBool("daed", true);
            hp = -101;
        }
    }

    public void SelfDestroy()
    {
        GameObject k;
        if (Drop.Length != 0)
        {
            foreach (var item in Drop)
            {
                if (item.chance >= Random.Range(0f, 100f))
                {
                    for (int i = 0; i < item.number; i++)
                    {
                        k = Instantiate(item.obj, transform);
                        k.transform.localPosition = Vector3.zero;
                        k.transform.rotation = Quaternion.identity;
                        k.transform.SetParent(null);
                    }
                }
            }
        }
        StopAllCoroutines();
        Destroy(gameObject);
    }
    public virtual void Jump(float jumpForce)
    {
        JumpTarget.rotation = Quaternion.Euler(0, 0, Random.Range(45, 135));
        rb.AddForce(JumpTarget.right * jumpForce, ForceMode2D.Impulse);
        an.Play("jump");
    }
    public void AddDamage(int d, bool byHand)
    {
        an.Play("damage");
        hp -= d==0 ? 0: ((byHand ? handArm : musicArm) < d ? d - (byHand ? handArm : musicArm) : 1);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hp > 0)
        {
            if (collision.gameObject.CompareTag("notePL"))
            {
                AddDamage(collision.gameObject.GetComponent<MusicNoteStart>().damage, false);
                collision.gameObject.GetComponent<MusicNoteStart>().StopAllCoroutines();
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                AddDamage(collision.gameObject.GetComponent<PlayerMover>().handCollideDamage, true);
            }
            NewOnCollisionEnter2D(collision);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hp > 0)
        {
            if (collision.CompareTag("HandHitter") && collision.gameObject.GetComponent<HandHitter>().LayerToAttack.Contains(gameObject.layer))
            {
                AddDamage(collision.gameObject.GetComponent<HandHitter>().Damage, true);
            }
            NewOnTriggerEnter2D(collision);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hp > 0)
        {
            NewOnTriggerStay2D(collision);
        }
    }
}

