using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Triggers/Music Note")]
public class MusicNoteStart : HandHitter
{
    public bool isRight = true;
    public Transform dir;
    public float force = 0;
    public float lifeTime = 0;
    protected virtual void Start()
    {
        StartCoroutine(ded());
    }
    IEnumerator ded()
    {
        yield return new WaitForSeconds(0.01f);
        try
        {
            GetComponent<Rigidbody2D>().AddForce((isRight ? dir.right : dir.up) * force, ForceMode2D.Impulse);
            yield return new WaitForSeconds(lifeTime);
            StopAllCoroutines();
            Destroy(gameObject);
        }
        finally
        {

        }
    }
    public override void Activate(NewEnemyBace entity)
    {
        base.Activate(entity);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
