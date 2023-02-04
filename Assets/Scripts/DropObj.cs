using UnityEngine;

[System.Serializable]
public class DropObj
{
    public GameObject obj;
    [Range(0f, 100f)]
    public float chance;
    public int number;
}
