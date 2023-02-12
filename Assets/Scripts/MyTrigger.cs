using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class MyTrigger : MonoBehaviour
{
    public int[] LayerToActivate;
    public abstract void Activate(NewEnemyBace entity);
}