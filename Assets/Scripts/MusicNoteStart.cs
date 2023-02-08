using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNoteStart : MonoBehaviour
{
    public bool isRight = true;
    public Transform dir;
    public float force = 0;
    public float lifeTime = 0;
    public float damage = 1;
    private void Start()
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
            Destroy(gameObject);
        }
        finally
        {

        }
    }
}
