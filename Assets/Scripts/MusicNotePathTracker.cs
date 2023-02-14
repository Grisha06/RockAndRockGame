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
        dir.LookAt(pl.transform, Vector3.forward);
        dir.Rotate(new Vector3(0, -90, 0));
        dir.Rotate(new Vector3(90, 0, 0));
    }
}
