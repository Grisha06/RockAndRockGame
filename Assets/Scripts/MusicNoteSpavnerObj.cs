using UnityEngine;

[System.Serializable]
public class BaseMusicNoteSpavnerObj
{
    public Transform MusicNoteSpavner;
    [Min(0)]
    public float SpawnTime = 0.1f;
    public float Force = 5;
    [Min(0)]
    public float Lifetime = 10;
    public int Damage = 1;
    public virtual void Attack(GameObject gobj)
    {

    }
}

[System.Serializable]
public class TreeSpavnerObj : BaseMusicNoteSpavnerObj
{
    public Entity[] ToSpawn;
}
[System.Serializable]
public class MusicNoteSpavnerObjPL : BaseMusicNoteSpavnerObj
{
    public int AmmoCost = 1;
}
