using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMover : NewEnemyBace
{
    public KeyObj[] controls;
    public float jumpForce = 1;
    private float angle;
    public float maxVelocity;
    public bool isGrounded;
    private int jumps;
    public int maxJumps = 3;
    private Vector2 mousePos;
    public Transform arrowObj;
    public SpriteRenderer arrowRend;
    public Sprite[] arrowSprites;
    public List<WeaponObg> weapon;
    public SpriteRenderer weaponSprite;
    public int weaponSelect = 0;
    public int MusicNoteSpavnerSelect;
    public int handCollideDamage = 1;
    private Camera mainCam;
    private Cameramower mainCamM;
    public PlayerOnAmmoChanged playerOnAmmoChanged;
    /// <summary>
    /// Don't use in Start() and NewStart()!
    /// FUCK! I DONT KNOW HOW TO DO SINGLETON RIGHT!!!!!
    /// </summary>
    [HideInInspector]
    public static PlayerMover single = null;

    protected override void NewAwake()
    {
        if (!single)
            single = this;
        playerOnAmmoChanged = new PlayerOnAmmoChanged();
        mainCam = Camera.main;
        mainCamM = mainCam.GetComponent<Cameramower>();
        StartCoroutine(Shooter());
    }
    protected override void NewUpdate()
    {
        arrowObj.rotation = Quaternion.LookRotation((Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position, Vector3.up);

        if (isGrounded) jumps = maxJumps;
        arrowRend.sprite = arrowSprites[jumps];
    }
    private void CheckGround() => isGrounded = Physics2D.OverlapCircle(tr.position, 0.42f, groundLayer);
    protected override void NewLateUpdate()
    {
        if (Input.GetKeyDown(KeyObj.FindInKeysArr(controls, "jump")) && (rb.velocity.magnitude < maxVelocity || maxVelocity < 0))
        {
            CheckGround();
            if (jumps == 0)
            {
                //mainCamM.Shake(.1f, .05f);
            }
            else
            {
                if (isGrounded)
                {
                    jumps = maxJumps;
                }
                else if (jumps > 0)
                {
                    jumps -= 1;
                }
                Jump(jumpForce);
            }
        }
        if (Input.GetKeyDown(KeyObj.FindInKeysArr(controls, "drop")))
        {
            OnInventoryDrop.Invoke();
        }
    }
    public override void Jump(float jumpForce)
    {
        rb.AddForce(arrowObj.forward * jumpForce, ForceMode2D.Impulse);
        OnJumped.Invoke();
        an.Play("jump");
    }
    private void Shoot()
    {
        GameObject mn = Instantiate(weapon[weaponSelect].MusicNote, weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].MusicNoteSpavner);
        weapon[weaponSelect].Attack(mn);
        mn.transform.rotation = Quaternion.identity;
        mn.transform.localScale = Vector3.one;
        MusicNoteStart mns = mn.GetComponent<MusicNoteStart>();
        mns.isRight = false;
        mns.dir = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].MusicNoteSpavner;
        mns.force = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].Force;
        mns.lifeTime = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].Lifetime;
        mns.damage = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].Damage;
        mn.transform.SetParent(null);
        weapon[weaponSelect].Ammo -= !weapon[weaponSelect].isAmmoDecreasing ? 0 : weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].AmmoCost;
        playerOnAmmoChanged.Invoke(weapon[weaponSelect].Ammo);
    }
    IEnumerator Shooter()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.01f);
        while (true)
        {
            yield return wfs;
            if (Input.GetKey(KeyObj.FindInKeysArr(controls, "decreaseWeaponSelect")))
            {
                weaponSelect += weaponSelect < weapon.Count - 1 ? 1 : 0;
                playerOnAmmoChanged.Invoke(weapon[weaponSelect].Ammo);
            }
            if (Input.GetKey(KeyObj.FindInKeysArr(controls, "increaseWeaponSelect")))
            {
                weaponSelect -= weaponSelect > 0 ? 1 : 0;
                playerOnAmmoChanged.Invoke(weapon[weaponSelect].Ammo);
            }
            weaponSprite.sprite = weapon[weaponSelect].sprite;
            if ((!weapon[weaponSelect].isAmmoDecreasing || weapon[weaponSelect].Ammo > 0) && hp > 0)
            {
                if (weapon[weaponSelect].isAutomatic && Input.GetKey(KeyObj.FindInKeysArr(controls, "attack")))
                {
                    yield return new WaitForSeconds(weapon[weaponSelect].ShootSpeed);
                    MusicNoteSpavnerSelect = 0;
                    an.Play("atack");
                    for (int i = 0; i < weapon[weaponSelect].musicNoteSpavnerObjs.Length; i++)
                    {
                        MusicNoteSpavnerSelect = i;
                        Shoot();
                        yield return new WaitForSeconds(weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].SpawnTime);
                    }
                    MusicNoteSpavnerSelect = 0;
                }
                if (Input.GetKeyDown(KeyObj.FindInKeysArr(controls, "attack")))
                {
                    MusicNoteSpavnerSelect = 0;
                    an.Play("atack");
                    for (int i = 0; i < weapon[weaponSelect].musicNoteSpavnerObjs.Length; i++)
                    {
                        MusicNoteSpavnerSelect = i;
                        Shoot();
                        yield return new WaitForSeconds(weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].SpawnTime);
                    }
                    MusicNoteSpavnerSelect = 0;
                    yield return new WaitForSeconds(weapon[weaponSelect].ShootSpeed);
                }
            }
        }
    }
    public override void SelfDestroy()
    {
        OnDie.Invoke();
        StopAllCoroutines();
        TryDestroyBossBar();
        SceneManager.LoadScene(0);
    }
    public override void AddDamage(float d, bool byHand)
    {
        base.AddDamage(d, byHand);
        if (hp > 0)
            an.Play("damage");
        else
            an.Play("die");
    }
    protected override void NewOnCollisionEnter2D(Collision2D collision)
    {
        CheckGround();
    }
}
