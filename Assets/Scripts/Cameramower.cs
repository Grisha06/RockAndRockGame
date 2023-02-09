using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramower : MonoBehaviour
{
    public Transform lookTo;
    public float CameraSpeed;
    public Shaker shaker;
    public bool isShaking = true;
    private Vector3 pos;
    private Transform tr;

    void Awake()
    {
        //lookTo = GameObject.FindGameObjectWithTag("Player").transform;
        tr = transform;
        if (lookTo != null) tr.position = lookTo.position;
    }
    void LateUpdate()
    {
        if (lookTo != null && !isShaking)
        {
            pos = new Vector3(lookTo.position.x, lookTo.position.y, -10f);
            tr.position = Vector3.Lerp(new Vector3(tr.position.x, tr.position.y, -10f), pos, Time.deltaTime * CameraSpeed);
        }
    }
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(shaker.ShakeC(duration, magnitude));
    }
}
