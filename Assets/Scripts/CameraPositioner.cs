using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    [SerializeField] private Transform slf;
    [SerializeField] private float dist = 1f;
    Vector2 mpos;
    private void Update()
    {
        mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        slf.position = (Vector2)transform.position - Vector2.ClampMagnitude((Vector2)transform.position - mpos, dist);
    }
}
