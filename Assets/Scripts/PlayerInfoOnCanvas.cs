using System;
using UnityEngine;
using UnityEngine.UI;
using NTC.Global.Cache;
using UnityEngine.Events;

[Serializable] public class EnemyOnHpChanged : UnityEvent<float> { }
[Serializable] public class PlayerOnAmmoChanged : UnityEvent<float> { }

public class PlayerInfoOnCanvas : MonoCache
{
    [SerializeField] private Text hp;
    [SerializeField] private Text ammo;
    private async void Start()
    {
        await System.Threading.Tasks.Task.Delay(500);
        PlayerMover.single.OnHpChanged.AddListener(ChangeHP); 
        PlayerMover.single.playerOnAmmoChanged.AddListener(ChangeAmmo);
        ChangeHP(PlayerMover.single.hp);
        ChangeAmmo(PlayerMover.single.weapon[PlayerMover.single.weaponSelect].Ammo);
    }
    private void ChangeHP(float hp_f) => hp.text = "HP = " + (hp_f > 0 ? hp_f : 0).ToString();
    private void ChangeAmmo(float ammo_f) => ammo.text = "Ammo = " + ammo_f.ToString();
}
