using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMover : NewEnemyBace
{
    public override void NewFixedUpdate() { }
    public override void NewOnTriggerStay2D(Collider2D collision) { }
    public override void NewOnTriggerEnter2D(Collider2D collision) { }

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
    public int handCollideDamage=1;
    private Camera mainCam;
    private Cameramower mainCamM;

    public override void NewStart()
    {
        mainCam = Camera.main;
        mainCamM = mainCam.GetComponent<Cameramower>();
        StartCoroutine(Shooter());
    }
    public override void NewUpdate()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePosZ = mousePos;
        mousePosZ.z = 0;
        Vector3 relativePos = mousePosZ - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        Vector3 e = rotation.eulerAngles;
        e = new Vector3(e.x, e.y, e.z);
        arrowObj.rotation = Quaternion.Euler(e);
        isGrounded = Physics2D.OverlapCircle(tr.position, 0.42f, groundLayer);
        if (isGrounded) jumps = maxJumps;
        arrowRend.sprite = arrowSprites[jumps];
    }
    public override void NewLateUpdate()
    {
        if (Input.GetKeyDown(KeyObj.FindInKeysArr(controls, "jump")) && (rb.velocity.magnitude < maxVelocity || maxVelocity < 0))
        {
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
    }
    public override void Jump(float jumpForce)
    {
        rb.AddForce(arrowObj.forward * jumpForce, ForceMode2D.Impulse);
        an.Play("jump");
    }
    private void Shoot()
    {
        GameObject mn = Instantiate(weapon[weaponSelect].MusicNote, weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].MusicNoteSpavner);
        weapon[weaponSelect].Attack(mn);
        mn.transform.rotation = Quaternion.identity;
        mn.transform.localScale = Vector3.one;
        mn.GetComponent<MusicNoteStart>().isRight = false;
        mn.GetComponent<MusicNoteStart>().dir = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].MusicNoteSpavner;
        mn.GetComponent<MusicNoteStart>().force = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].Force;
        mn.GetComponent<MusicNoteStart>().lifeTime = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].Lifetime;
        mn.GetComponent<MusicNoteStart>().damage = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].Damage;
        mn.transform.SetParent(null);
        weapon[weaponSelect].Ammo -= !weapon[weaponSelect].isAmmoDecreasing ? 0 : weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].AmmoCost;
    }
    IEnumerator Shooter()
    {
        bool h = false;
        WaitForSeconds wfs = new WaitForSeconds(0.01f);
        while (true)
        {
            yield return wfs;
            if (Input.GetKey(KeyObj.FindInKeysArr(controls, "decreaseWeaponSelect")))
            {
                weaponSelect += weaponSelect < weapon.Count - 1 ? 1 : 0;
            }
            if (Input.GetKey(KeyObj.FindInKeysArr(controls, "increaseWeaponSelect")))
            {
                weaponSelect -= weaponSelect > 0 ? 1 : 0;
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
                if (!weapon[weaponSelect].isAutomatic && Input.GetKeyDown(KeyObj.FindInKeysArr(controls, "attack")))
                {
                    h = true;
                    t = 0;

                }
                if (t >= weapon[weaponSelect].ShootSpeed && h)
                {
                    //yield return new WaitForSeconds(0.1f);
                    MusicNoteSpavnerSelect = 0;
                    an.Play("atack");
                    for (int i = 0; i < weapon[weaponSelect].musicNoteSpavnerObjs.Length; i++)
                    {
                        MusicNoteSpavnerSelect = i;
                        Shoot();
                        yield return new WaitForSeconds(weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].SpawnTime);
                    }
                    MusicNoteSpavnerSelect = 0;
                    h = false;
                }
            }
        }
    }
    public override void SelfDestroy()
    {
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
    public override void NewOnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("destroyOnCollPl"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("hp"))
        {
            hp += 2;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("ammo"))
        {
            weapon[weaponSelect].Ammo += 10;
            Destroy(collision.gameObject);
        }
    }
}
