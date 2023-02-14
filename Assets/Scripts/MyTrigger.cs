using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;

[RequireComponent(typeof(Collider2D))]
public abstract class MyTrigger : MonoCache
{
    public int[] LayerToActivate;
    protected NewEnemyBace neb = null;
    public virtual void Activate(NewEnemyBace entity)
    {
        neb = entity;
    }
    public virtual void Diactivate(NewEnemyBace entity)
    {
        neb = null;
    }
}