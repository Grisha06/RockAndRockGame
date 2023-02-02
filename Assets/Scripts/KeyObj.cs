using UnityEngine;

[System.Serializable]
public class KeyObj
{
    public KeyCode key;
    public string name;

    public static KeyCode FindInKeysArr(KeyObj[] a, string name)
    {
        foreach (var item in a)
        {
            if (item.name == name)
            {
                return item.key;
            }
        }
        return KeyCode.None;
    }
}
