using UnityEngine;

[System.Serializable]
public class WeaponObg
{
    public bool isAutomatic = false;
    public bool isAmmoDecreasing = true;
    public bool isReloadable = false;
    [Min(0)]
    public int Ammo = 0;
    [Min(0)]
    public int ClipAmmo = 0;
    [Min(1)]
    public int MaxClipAmmo = 100;
    [Min(0)]
    public float ReloadSpeed = 1;
    [Min(0)]
    public float ShootSpeed = 0.08f;
    public MusicNoteSpavnerObjPL[] musicNoteSpavnerObjs;
    public GameObject MusicNote;
    public Sprite sprite;
    public virtual void Attack(GameObject gobj) { }
}