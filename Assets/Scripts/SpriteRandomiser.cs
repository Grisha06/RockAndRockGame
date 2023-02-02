using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomiser : MonoBehaviour
{
    public List<Sprite> Sprits;
    public bool rotate = true;
    public bool color = false;
    [HideInInspector]
    public bool color_value = false;
    [HideInInspector]
    public float color_value_float = 1;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Sprits[Random.Range(0, Sprits.Count)];
        if (rotate) transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)));
        if (color)
        {
            GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0f, 1f), color_value ? Random.Range(0f, 1f) : color_value_float);
        }
    }
}
