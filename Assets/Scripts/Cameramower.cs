using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;

public class Cameramower : MonoCache
{
    public Transform lookTo;
    public float CameraSpeed;
    public Shaker shaker;
    public bool isShaking = true;
    public bool isTargeted = false;
    public float OrtSize;
    [HideInInspector]
    public Camera cam;
    private Vector3 pos;
    private Transform tr;

    void Awake()
    {
        //lookTo = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();
        OrtSize=cam.orthographicSize;
        tr = transform;
        if (lookTo != null) tr.position = lookTo.position;
    }
    protected override void LateRun()
    {
        if (lookTo != null && !isShaking && !isTargeted)
        {
            pos = new Vector3(lookTo.position.x, lookTo.position.y, -10f);
            tr.position = Vector3.Lerp(new Vector3(tr.position.x, tr.position.y, -10f), pos, Time.deltaTime * CameraSpeed);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, OrtSize, Time.deltaTime * CameraSpeed);
        }
    }
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(shaker.ShakeC(duration, magnitude));
    }
}
