using UnityEngine;

[AddComponentMenu("Enemies/Plant")]
public class EnemyPlant : EntityAttakable
{
    public bool rotateMusicNoteSpawners = true;
    protected override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
        WingsPos.SetActive(false);

        enemyBaceAction = attackIfRad && Vector2.Distance(PlayerMover.single.tr.position, tr.position) < attackRadius ? EnemyBaceActions.Attack : EnemyBaceActions.Run;
        if (rotateMusicNoteSpawners && enemyBaceAction == EnemyBaceActions.Attack)
        {
            foreach (var item in MusicNoteSpavner)
            {
                item.MusicNoteSpavner.LookAt(PlayerMover.single.transform, Vector3.forward);
                item.MusicNoteSpavner.Rotate(new Vector3(0, -90, 0));
                item.MusicNoteSpavner.Rotate(new Vector3(90, 0, 0));
            }
        }
    }
}
