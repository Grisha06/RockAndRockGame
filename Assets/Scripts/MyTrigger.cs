using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;

[RequireComponent(typeof(Collider2D))]
public abstract class MyTrigger : MonoCache
{
    public int[] LayerToActivate;
    public abstract void Activate(NewEnemyBace entity);
}