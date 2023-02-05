using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Plants/Cell")]
public class EnemyPlantCell : NewEnemyBace
{
    public GameObject Prisoner;
    public SpriteRenderer PrisonerPlace;

    public override void NewUpdate() { }
    public override void NewOnCollisionEnter2D(Collision2D collision) { }
    public override void NewFixedUpdate() 
    {
        PrisonerPlace.sprite = Prisoner.GetComponent<NewEnemyBace>().sr.sprite;
    }
    public override void NewOnTriggerStay2D(Collider2D collision) { }
    public override void NewOnTriggerEnter2D(Collider2D collision) { }
    public override void NewStart() { }

    public override void SelfDestroy()
    {
        GameObject k = Instantiate(Prisoner, transform);
        k.transform.localPosition = Vector3.zero;
        k.transform.rotation = Quaternion.identity;
        k.transform.SetParent(null);
        base.SelfDestroy();
    }
}
