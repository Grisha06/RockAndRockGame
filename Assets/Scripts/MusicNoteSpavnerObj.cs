using UnityEngine;

[System.Serializable]
public class BaseMusicNoteSpavnerObj
{
    [SerializeField] public Transform MusicNoteSpavner;
    [SerializeField] public float SpavnTime = 0.1f;
    [SerializeField] public float force = 5;
    [SerializeField] public float lifeTime = 10;
    [SerializeField] public int damage = 1;
    public virtual void Attack(GameObject gobj)
    {

    }
}

[System.Serializable]
public class MusicNoteSpavnerObj : BaseMusicNoteSpavnerObj
{

}
