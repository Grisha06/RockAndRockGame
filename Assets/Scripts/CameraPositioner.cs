using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;

public class CameraPositioner : MonoCache
{
    [SerializeField] private Transform slf;
    [SerializeField] private float dist = 1f;
    Vector2 mpos;
    protected override void Run()
    {
        mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        slf.position = (Vector2)transform.position - Vector2.ClampMagnitude((Vector2)transform.position - mpos, dist);
    }
}
