using UnityEngine;

[System.Serializable]
public class WeaponObg
{
    public bool auto = false;
    public bool inf_ammo = true;
    [Min(0)]
    public int ammo = 0;
    [Min(0)]
    public int ammo_by_shoot = 1;
    [Min(0)]
    public float shoot_speed = 0.08f;
    public MusicNoteSpavnerObj[] musicNoteSpavnerObjs;
    public GameObject MusicNote;
    public Sprite sprite;
    public virtual void Attack(GameObject gobj)
    {

    }
}
