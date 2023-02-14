using UnityEngine;

[AddComponentMenu("Triggers/Music Note Tracker")]
public class MusicNotePathTracker : MusicNoteStart
{
    private PlayerMover pl;

    protected override void Start()
    {
        base.Start();
        pl = FindObjectOfType<PlayerMover>();
    }
    protected override void Run()
    {
        Vector2 r = Quaternion.LookRotation(transform.position - pl.tr.position, Vector3.up).eulerAngles * Mathf.Deg2Rad;
        rb.velocity = new Vector2((transform.position.x < pl.tr.position.x ? 1f : -1f) * Mathf.Cos(r.x), Mathf.Sin(r.x));
    }
}
