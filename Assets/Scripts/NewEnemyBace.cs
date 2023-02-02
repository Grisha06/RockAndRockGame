using UnityEngine;

public abstract class NewEnemyBace : MonoBehaviour
{
    [Header("Inheritanced fields")]
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
    [HideInInspector]
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
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        NewStart();
    }
    public abstract void NewStart();
    public abstract void NewFixedUpdate();
    public abstract void NewUpdate();
    public abstract void NewOnCollisionEnter2D(Collision2D collision);

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
    public void Jump(float jumpForce)
    {
        JumpTarget.rotation = Quaternion.Euler(0, 0, Random.Range(45, 135));
        rb.AddForce(JumpTarget.right * jumpForce, ForceMode2D.Impulse);
        an.Play("jump");
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
            NewOnCollisionEnter2D(collision);
        }
    }
}

