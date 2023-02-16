using UnityEngine;

[AddComponentMenu("Triggers/Ammo Trigger")]
public class AmmoTrigger : DestroyOnCollisionTrigger
{
    public int Ammo = 10;
    public override void Activate(NewEnemyBace entity)
    {
        base.Activate(entity);
        ((PlayerMover)entity).weapon[((PlayerMover)entity).weaponSelect].Ammo += Ammo;
        PlayerInfoOnCanvas.textUpdate.Invoke(((PlayerMover)entity).hp, ((PlayerMover)entity).weapon[((PlayerMover)entity).weaponSelect].Ammo);
        base.Activate(entity);
    }
}