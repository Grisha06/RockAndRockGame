using UnityEngine;

[System.Serializable]
public class WeaponObg
{
    public bool isAutomatic = false;
    public bool isAmmoDecreasing = true;
    [Min(0)]
    public int Ammo = 0;
    [Min(0)]
    public float ShootSpeed = 0.08f;
    public MusicNoteSpavnerObjPL[] musicNoteSpavnerObjs;
    public GameObject MusicNote;
    public Sprite sprite;
    public virtual void Attack(GameObject gobj) { }
}