using UnityEngine;

[AddComponentMenu("Triggers/Ammo Trigger")]
public class AmmoTrigger : DestroyOnCollisionTrigger
{
    public int Ammo = 10;
    public override void Activate(NewEnemyBace entity)
    {
        ((PlayerMover)entity).weapon[((PlayerMover)entity).weaponSelect].Ammo += Ammo;
        base.Activate(entity);
    }
}