using UnityEngine;

[System.Serializable]
public class BaseMusicNoteSpavnerObj
{
    [SerializeField] public Transform MusicNoteSpavner;
    [Min(0)]
    [SerializeField] public float SpawnTime = 0.1f;
    [SerializeField] public float Force = 5;
    [Min(0)]
    [SerializeField] public float Lifetime = 10;
    [SerializeField] public int Damage = 1;
    public virtual void Attack(GameObject gobj)
    {

    }
}

[System.Serializable]
public class MusicNoteSpavnerObj : BaseMusicNoteSpavnerObj
{

}
[System.Serializable]
public class MusicNoteSpavnerObjPL : BaseMusicNoteSpavnerObj
{
    [SerializeField] public int AmmoCost = 1;
}
