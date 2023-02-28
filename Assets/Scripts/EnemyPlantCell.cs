using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Plants/Cell")]
public class EnemyPlantCell : Entity
{
    public GameObject Prisoner;
    public SpriteRenderer PrisonerPlace;

    protected override void NewFixedUpdate() 
    {
        PrisonerPlace.sprite = Prisoner.GetComponent<Entity>().sr.sprite;
    }

    public override void SelfDestroy()
    {
        GameObject k = Instantiate(Prisoner, transform);
        k.transform.localPosition = Vector3.zero;
        k.transform.rotation = Quaternion.identity;
        k.transform.SetParent(null);
        base.SelfDestroy();
    }
}
