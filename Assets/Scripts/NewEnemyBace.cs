using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using NTC.Global.Cache;


public abstract class EnemyBaceAttakable : NewEnemyBace
{
    public float attackRadius;
    public BaseMusicNoteSpavnerObj[] MusicNoteSpavner;
    public bool sameTimeAttack = false;
    public float attackTime;
    [HideInInspector]
    public int MusicNoteSpavnerSelNum = 0;
    public bool attackIfRad = true;

    public sealed override void Start()
    {
        base.Start();
        StartCoroutine(AttackingEnumerator());
    }
    public void SpawnM(int mnssn)
    {
        GameObject mn = Instantiate(MusicNote, MusicNoteSpavner[mnssn].MusicNoteSpavner);
        MusicNoteSpavner[mnssn].Attack(mn);
        mn.transform.rotation = Quaternion.identity;
        mn.transform.localScale = Vector3.one;
        MusicNoteStart mns = mn.GetComponent<MusicNoteStart>();
        mns.dir = MusicNoteSpavner[mnssn].MusicNoteSpavner;
        mns.force = MusicNoteSpavner[mnssn].Force;
        mns.lifeTime = MusicNoteSpavner[mnssn].Lifetime;
        mns.damage = MusicNoteSpavner[mnssn].Damage;
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
                    yield return new WaitForSeconds(0.25f);
                    if (hp > 0)
                        an.Play("spawnMN_nospawn");
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

[RequireComponent(typeof(Rigidbody2D)), DisallowMultipleComponent]
public abstract class NewEnemyBace : MonoCache, IDamagable
{
    [Header("Inheritanced fields")]
    public string EntityName = "/n";
    [SerializeField]
    private TextMeshProUGUI nameText;
    public EnemyBaceActions enemyBaceAction;
    public bool Flipable = false;
    public bool Flip = false;
    public bool FlipLocalScale = false;
    [SerializeField]
    private float health;
    [HideInInspector]
    public float maxHealth;

    public float hp
    {
        get { return health; }
        set
        {
            health = value;
            if (health > maxHealth)
                maxHealth = health;
        }
    }
    [Min(0)]
    public float musicArm;
    [Min(0)]
    public float handArm;
    public DropObj[] Drop;
    public GameObject MusicNote;
    public Transform JumpTarget;
    public GameObject WingsPos;
    public LayerMask groundLayer;
    public Transform pl;
    [HideInInspector]
    public Animator an;
    [HideInInspector]
    public Transform tr;
    [HideInInspector]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    [HideInInspector]
    public float t = 0;
    [Header("Custom fields")]
    public bool dont_totch_me;

    public virtual void Start()
    {
        maxHealth = health;
        tr = transform;
        if (gameObject.CompareTag("Player"))
            pl = null;
        else
            pl = GameObject.FindGameObjectWithTag("Player").transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        an = gameObject.GetComponent<Animator>();
        if (nameText)
            nameText.transform.parent.gameObject.SetActive(true);
        NewStart();
    }
    public virtual void NewStart() { }
    public virtual void NewFixedUpdate() { }
    public virtual void NewUpdate() { }
    public virtual void NewLateUpdate() { }
    public virtual void NewOnCollisionEnter2D(Collision2D collision) { }
    public virtual void NewOnTriggerStay2D(Collider2D collision) { }
    public virtual void NewOnTriggerEnter2D(Collider2D collision) { }

    protected override void FixedRun()
    {
        if (hp > 0)
        {
            NewFixedUpdate();
        }
    }
    protected override void Run()
    {
        if (gameObject.CompareTag("Player"))
            t += Time.deltaTime;
        if (hp > 0)
        {
            NewUpdate();
        }
    }
    protected override void LateRun()
    {
        if (hp > 0)
        {
            if (!gameObject.CompareTag("Player") && rb)
            {
                if (!FlipLocalScale)
                    sr.flipX = (Flip ? rb.velocity.x < 0 : rb.velocity.x > 0) && rb.velocity.x != 0 && Flipable;
                else
                    sr.gameObject.transform.localScale = new Vector3(sr.gameObject.transform.localScale.x * ((Flip ? rb.velocity.x < 0 : rb.velocity.x > 0) && rb.velocity.x != 0 && Flipable ? -1 : 1), sr.gameObject.transform.localScale.y, sr.gameObject.transform.localScale.z);
                nameText.text = EntityName == "/n" ? "" : EntityName;
            }
            NewLateUpdate();
        }
        if (hp <= 0 && hp > -100 && !gameObject.CompareTag("Player"))
        {
            an.Play("die");
            an.SetBool("daed", true);
            hp = -101;
        }
    }
    public void TryDestroyBossBar()
    {

        try
        {
            GetComponent<BossBar>().use = false;
            Destroy(GetComponent<BossBar>().bbh.BossBarObj);
        }
        catch (NullReferenceException)
        {

        }
    }

    public virtual void SelfDestroy()
    {
        DropObj.Drop(transform.position, Drop);
        StopAllCoroutines();
        TryDestroyBossBar();
        Destroy(gameObject);
    }
    public virtual void Jump(float jumpForce)
    {
        JumpTarget.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(45, 135));
        rb.AddForce(JumpTarget.right * jumpForce, ForceMode2D.Impulse);
        an.Play("jump");
    }
    public virtual void AddDamage(float d, bool byHand)
    {
        an.Play("damage");
        hp -= d == 0 ? 1 : (d / (float)(byHand ? (handArm == 0 ? 1 : Math.Max(handArm, 1f)) : (musicArm == 0 ? 1 : Math.Max(musicArm, 1f))));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hp > 0)
        {
            NewOnCollisionEnter2D(collision);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hp > 0)
        {
            MyTrigger mt = collision.GetComponent<MyTrigger>();
            if (mt && mt.LayerToActivate.Contains(gameObject.layer))
            {
                mt.Activate(this);
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

public interface IDamagable
{
    public void AddDamage(float damage, bool byHand);
}