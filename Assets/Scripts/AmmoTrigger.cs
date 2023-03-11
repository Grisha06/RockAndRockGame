using UnityEngine;

[AddComponentMenu("Triggers/Ammo Trigger")]
public class AmmoTrigger : DestroyOnCollisionTrigger
{
    public int Ammo = 10;
    public override void Activate(Entity entity)
    {
        if(PlayerMover.single.weapon.Count != 0)
        {
            base.Activate(entity);
            PlayerMover.single.weapon[PlayerMover.single.weaponSelect].Ammo += Ammo;
            PlayerMover.single.playerOnAmmoChanged.Invoke(PlayerMover.single.weapon[PlayerMover.single.weaponSelect].Ammo, PlayerMover.single.weapon[PlayerMover.single.weaponSelect].ClipAmmo, PlayerMover.single.weapon[PlayerMover.single.weaponSelect].MaxClipAmmo, PlayerMover.single.weapon.Count == 0);

            base.Activate(entity);
        }
    }
}