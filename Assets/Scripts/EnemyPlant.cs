using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Enemies/Plant")]
public class EnemyPlant : EnemyBaceAttakable
{
    public override void NewFixedUpdate()
    {
        base.NewFixedUpdate();
        foreach (var item in MusicNoteSpavner)
        {
            item.MusicNoteSpavner.LookAt(pl.transform, Vector3.forward);
            item.MusicNoteSpavner.Rotate(new Vector3(0,-90,0));
            item.MusicNoteSpavner.Rotate(new Vector3(90,0,0));
        }
        WingsPos.SetActive(false);
        if (attackIfRad && Vector2.Distance(pl.position, tr.position) < attackRadius)
        {
            enemyBaceAction = EnemyBaceActions.Attack;
        }
    }
}
