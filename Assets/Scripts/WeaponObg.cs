using UnityEngine;

[System.Serializable]
public abstract class WeaponAbstract
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
    public abstract void Attack(GameObject gobj);
}

[System.Serializable]
public class WeaponObg : WeaponAbstract
{
    public override void Attack(GameObject gobj)
    {

    }
}