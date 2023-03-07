using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Walker Carrier")]
public class EnemyWalkerCarrier : Enemy_Walker_Rock
{
    [SerializeField]
    protected Entity CarringGameObject;
    [SerializeField]
    protected Transform ToCarringTransform;
    private Entity cgo;
    private RigidbodyConstraints2D cgoConstraints;
    protected override void NewAwake()
    {
        base.NewAwake();
        if (CarringGameObject)
        {
            cgo = Instantiate(CarringGameObject);
            cgo.tr.SetParent(ToCarringTransform);
            cgo.tr.localPosition = Vector3.zero;
            cgoConstraints = cgo.rb.constraints;
            cgo.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    protected override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
        if (cgo) cgo.tr.position = ToCarringTransform.position;
    }
    public override void SelfDestroy()
    {
        if (cgo)
        {
            cgo.tr.SetParent(null);
            cgo.tr.position = ToCarringTransform.position;
            cgo.rb.constraints = cgoConstraints;
        }
        base.SelfDestroy();
    }
}
