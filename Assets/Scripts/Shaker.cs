using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;

public class Shaker : MonoCache
{
    public IEnumerator ShakeC(float duration, float magnitude)
    {
        Cameramower cm = GetComponent<Cameramower>();
        Vector2 origPos = transform.position;
        float elapsed = 0f;
        cm.isShaking = true;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude + transform.position.x;
            float y = Random.Range(-1f, 1f) * magnitude + transform.position.y;
            transform.position = new Vector3(x, y, -10);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cm.isShaking = false;
        transform.position = origPos;
    }
    public void Shake(float duration)
    {
        StartCoroutine(ShakeC(duration, duration));
    }
}
