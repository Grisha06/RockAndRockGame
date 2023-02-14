using UnityEngine;

[AddComponentMenu("Enemies/Plant")]
public class EnemyPlant : EnemyBaceAttakable
{
    public bool rotateMusicNoteSpawners = true;
    public override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
        WingsPos.SetActive(false);
        if (attackIfRad)
        {
            enemyBaceAction = Vector2.Distance(pl.position, tr.position) < attackRadius? EnemyBaceActions.Attack: EnemyBaceActions.Run;
        }
        if (rotateMusicNoteSpawners && enemyBaceAction == EnemyBaceActions.Attack)
        {
            foreach (var item in MusicNoteSpavner)
            {
                item.MusicNoteSpavner.LookAt(pl.transform, Vector3.forward);
                item.MusicNoteSpavner.Rotate(new Vector3(0, -90, 0));
                item.MusicNoteSpavner.Rotate(new Vector3(90, 0, 0));
            }
        }
    }
}
