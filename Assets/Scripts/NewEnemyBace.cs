using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using NTC.Global.Cache;
using UnityEngine.Events;

public abstract class EntityAttakable : Entity
{
    public float attackRadius;
    public BaseMusicNoteSpavnerObj[] MusicNoteSpavner;
    public bool sameTimeAttack = false;
    public float attackTime;
    [HideInInspector]
    public int MusicNoteSpavnerSelNum = 0;
    public bool attackIfRad = true;

    public sealed override void Awake()
    {
        StartCoroutine(AttackingEnumerator());
        base.Awake();
    }
    protected override void NewOnDrawGizmosSelected()
    {
        //if (attackIfRad)
            //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, attackRadius);
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
        yield return new WaitForSeconds(0.01f);
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

public enum EnemyBaceActions
{
    Run,
    Attack,
    None
}

[Serializable] public class EnemyOnTriggerEnter : UnityEvent<InventoryTrigger> { }
[Serializable] public class EnemyOnInventoryDrop : UnityEvent<Inventory> { }

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoCache, IDamagable
{
    [Header("Inheritanced fields")]
    public string EntityName = "/n";
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    protected EnemyBaceActions enemyBaceAction;
    [SerializeField]
    protected bool Flipable = false;
    [SerializeField]
    protected bool Flipped = false;
    [SerializeField]
    protected bool FlipLocalScale = false;
    [SerializeField]
    protected float health;
    [HideInInspector]
    public float maxHealth;
    [HideInInspector]
    public EnemyOnHpChanged OnHpChanged;
    [HideInInspector]
    public EnemyOnTriggerEnter OnInvTriggerEntered;
    [HideInInspector]
    public UnityEvent OnJumped;
    [HideInInspector]
    public UnityEvent OnDie;
    [HideInInspector]
    public UnityEvent OnInvItemRemoved;
    public bool dynamicMaxHp = true;

    public float hp
    {
        get => GetHp();
        set => SetHp(value);
    }
    protected virtual void SetHp(float value)
    {
        if (!dynamicMaxHp)
        {
            health = value;
            if (health > maxHealth)
                maxHealth = health;
        }
        else
        {
            health = Mathf.Min(maxHealth, value);
        }
    }
    protected virtual float GetHp() => health;

    [Min(0)]
    public float musicArm;
    [Min(0)]
    public float handArm;
    public DropObj[] Drop;
    public GameObject MusicNote;
    public Transform JumpTarget;
    public GameObject WingsPos;
    public LayerMask groundLayer;
    [HideInInspector]
    public Animator an;
    [HideInInspector]
    public Transform tr;
    [HideInInspector]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    [SerializeField]
    private bool drawGizmos = false;
    [Header("Custom fields")]
    public bool dont_totch_me;

    [ContextMenu("Die")]
    private void ContextMenuDie()
    {
        AddDamage(hp, DamageType.Generic);
    }
    private void OnDrawGizmosSelected()
    {
        if (drawGizmos)
            NewOnDrawGizmosSelected();
    }
    protected virtual void NewOnDrawGizmosSelected() { }

    public virtual void Awake()
    {
        maxHealth = health;
        tr = transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        an = gameObject.GetComponent<Animator>();
        if (nameText)
            nameText.transform.parent.gameObject.SetActive(true);
        OnHpChanged = new EnemyOnHpChanged();
        OnJumped = new UnityEvent();
        OnDie = new UnityEvent();
        OnInvTriggerEntered = new EnemyOnTriggerEnter();
        NewAwake();
    }
    protected virtual void NewAwake() { }
    protected virtual void NewFixedUpdate() { }
    protected virtual void NewUpdate() { }
    protected virtual void NewLateUpdate() { }
    protected virtual void NewOnCollisionEnter2D(Collision2D collision) { }
    protected virtual void NewOnTriggerStay2D(Collider2D collision) { }
    protected virtual void NewOnTriggerEnter2D(Collider2D collision) { }
    protected virtual void NewOnTriggerExit2D(Collider2D collision) { }

    protected sealed override void FixedRun()
    {
        if (hp > 0)
        {
            NewFixedUpdate();
        }
    }
    protected sealed override void Run()
    {
        if (hp > 0)
        {
            NewUpdate();
        }
    }
    protected virtual void DoFlip()
    {
        if (!Mathf.Approximately(rb.velocity.x, 0)) Flip(rb.velocity.x > 0);
    }
    protected void Flip(bool _flip)
    {
        if (Flipped)
            _flip = !_flip;

        if (!FlipLocalScale)
            sr.flipX = _flip;
        else
            sr.gameObject.transform.localScale = new Vector3(
                sr.gameObject.transform.localScale.x * (_flip ? -1 : 1
                ), sr.gameObject.transform.localScale.y, sr.gameObject.transform.localScale.z);
    }
    protected sealed override void LateRun()
    {
        if (hp > 0)
        {
            if (nameText) nameText.transform.parent.rotation = Quaternion.identity;

            if (rb && Flipable)
            {
                DoFlip();
            }
            if (nameText)
                nameText.text = EntityName == "/n" ? "" : EntityName;
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
        OnDie.Invoke();
        DropObj.Drop(transform.position, Drop);
        StopAllCoroutines();
        TryDestroyBossBar();
        Destroy(gameObject);
    }
    public virtual void Jump(float jumpForce)
    {
        JumpTarget.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(45, 135));
        rb.AddForce(JumpTarget.right * jumpForce, ForceMode2D.Impulse);
        OnJumped.Invoke();
        an.Play("jump");
    }
    public enum DamageType
    {
        Hand, Music, Generic
    }
    protected virtual float CalculateDamage(float d, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Hand:
                return (d == 0 ? 1 : (d / (float)(handArm == 0 ? 1 : Math.Max(handArm, 1f))));
            case DamageType.Music:
                return (d == 0 ? 1 : (d / (float)(musicArm == 0 ? 1 : Math.Max(musicArm, 1f))));
            case DamageType.Generic:
                return (d == 0 ? 1 : d);
            default:
                break;
        }
        return 0f;
    }
    public virtual void AddDamage(float d, DamageType damageType)
    {
        an.Play("damage");
        d = Mathf.Max(d, 0);
        hp = hp - CalculateDamage(d, damageType);
        OnHpChanged.Invoke(hp);
    }
    public virtual void Heal(float d)
    {
        d = Mathf.Max(d, 0);
        hp = hp + d;
        OnHpChanged.Invoke(hp);
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
        if (hp > 0 && collision.gameObject.layer != 12)
        {
            MyTrigger mt = collision.GetComponent<MyTrigger>();
            if (mt && mt.LayerToActivate.Contains(gameObject.layer))
            {
                mt.Activate(this);
                mt.UpdateNebActivate(this);
            }
            NewOnTriggerEnter2D(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hp > 0)
        {
            MyTrigger mt = collision.GetComponent<MyTrigger>();
            if (mt && mt.LayerToActivate.Contains(gameObject.layer))
            {
                mt.Diactivate(this);
                mt.UpdateNebDiactivate();
            }
            NewOnTriggerExit2D(collision);
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
    public void AddDamage(float damage, Entity.DamageType damageType);
    public void Heal(float damage);
}