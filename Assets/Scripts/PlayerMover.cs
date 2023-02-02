using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMover : MonoBehaviour
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
    public LayerMask groundLayer;
    public Animator an;
    public List<WeaponObg> weapon;
    public SpriteRenderer weaponSprite;
    public int weaponSelect = 0;
    public int MusicNoteSpavnerSelect;
    private Transform tr;
    private Rigidbody2D rb;
    private Camera mainCam;
    private Cameramower mainCamM;
    [SerializeField]
    private float t = 0;

    [Header("HP")]
    public int hp = 10;
    public int musicRes = 0;
    public int handRes = 5;

    private void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        mainCamM = mainCam.GetComponent<Cameramower>();
        StartCoroutine(Shooter());
    }
    private void Update()
    {
        t += Time.deltaTime;
        if (hp > 0)
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
        if (hp < 0 && hp > -100)
        {
            an.Play("die");
            hp = -101;
        }
    }
    private void LateUpdate()
    {
        if (hp > 0)
        {
            if (Input.GetKeyDown(KeyObj.FindInKeysArr(controls, "jump")) && (rb.velocity.magnitude < maxVelocity || maxVelocity < 0))
            {
                if (jumps == 0)
                {
                    mainCamM.Shake(.1f, .05f);
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
                    Jump();
                }
            }
        }
    }
    private void Jump()
    {
        rb.AddForce(arrowObj.forward * jumpForce, ForceMode2D.Impulse);
        an.Play("jump");
    }
    private void Shoot()
    {
        GameObject mn = Instantiate(weapon[weaponSelect].MusicNote, weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].MusicNoteSpavner);
        mn.transform.rotation = Quaternion.identity;
        mn.transform.localScale = Vector3.one;
        mn.GetComponent<MusicNoteStart>().isRight = false;
        mn.GetComponent<MusicNoteStart>().dir = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].MusicNoteSpavner;
        mn.GetComponent<MusicNoteStart>().force = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].force;
        mn.GetComponent<MusicNoteStart>().lifeTime = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].lifeTime;
        mn.GetComponent<MusicNoteStart>().damage = weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].damage;
        mn.transform.SetParent(null);
        weapon[weaponSelect].ammo -= weapon[weaponSelect].inf_ammo ? 0 : weapon[weaponSelect].ammo_by_shoot;
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
            if ((weapon[weaponSelect].inf_ammo || weapon[weaponSelect].ammo > 0) && hp > 0)
            {
                if (weapon[weaponSelect].auto && Input.GetKey(KeyObj.FindInKeysArr(controls, "attack")))
                {
                    yield return new WaitForSeconds(weapon[weaponSelect].shoot_speed);
                    MusicNoteSpavnerSelect = 0;
                    an.Play("atack");
                    for (int i = 0; i < weapon[weaponSelect].musicNoteSpavnerObjs.Length; i++)
                    {
                        MusicNoteSpavnerSelect = i;
                        Shoot();
                        yield return new WaitForSeconds(weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].SpavnTime);
                    }
                    MusicNoteSpavnerSelect = 0;
                }
                if (!weapon[weaponSelect].auto && Input.GetKeyDown(KeyObj.FindInKeysArr(controls, "attack")))
                {
                    h = true;
                    t = 0;

                }
                if (t >= weapon[weaponSelect].shoot_speed && h)
                {
                    //yield return new WaitForSeconds(0.1f);
                    MusicNoteSpavnerSelect = 0;
                    an.Play("atack");
                    for (int i = 0; i < weapon[weaponSelect].musicNoteSpavnerObjs.Length; i++)
                    {
                        MusicNoteSpavnerSelect = i;
                        Shoot();
                        yield return new WaitForSeconds(weapon[weaponSelect].musicNoteSpavnerObjs[MusicNoteSpavnerSelect].SpavnTime);
                    }
                    MusicNoteSpavnerSelect = 0;
                    h = false;
                }
            }
        }
    }
    public void SelfDestroy()
    {
        SceneManager.LoadScene(0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hp > 0)
        {
            if (collision.gameObject.CompareTag("note"))
            {
                hp -= musicRes < collision.gameObject.GetComponent<MusicNoteStart>().damage ? collision.gameObject.GetComponent<MusicNoteStart>().damage - musicRes : 1;
                if (hp > 0)
                    an.Play("damage");
                else
                    an.Play("die");
                Destroy(collision.gameObject);
            }
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
                weapon[weaponSelect].ammo += 10;
                Destroy(collision.gameObject);
            }
        }
    }
}
