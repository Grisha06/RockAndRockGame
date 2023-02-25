using System;
using UnityEngine;
using UnityEngine.UI;
using NTC.Global.Cache;
using UnityEngine.Events;
using TMPro;

[Serializable] public class EnemyOnHpChanged : UnityEvent<float> { }
[Serializable] public class PlayerOnAmmoChanged : UnityEvent<float,float,float> { }

public class PlayerInfoOnCanvas : MonoCache
{
    [SerializeField] private Image hp;
    [SerializeField] private Image hp2;
    [SerializeField] private TextMeshProUGUI ammo;
    [SerializeField] private Image ammoSpiner;
    private async void Start()
    {
        await System.Threading.Tasks.Task.Delay(500);
        PlayerMover.single.OnHpChanged.AddListener(ChangeHP);
        PlayerMover.single.playerOnAmmoChanged.AddListener(ChangeAmmo);
        ChangeHP(PlayerMover.single.hp);
        ChangeAmmo(PlayerMover.single.weapon[PlayerMover.single.weaponSelect].Ammo, PlayerMover.single.weapon[PlayerMover.single.weaponSelect].ClipAmmo, PlayerMover.single.weapon[PlayerMover.single.weaponSelect].MaxClipAmmo);
    }
    private void ChangeHP(float hp_f)
    {
        hp.rectTransform.localScale = new Vector3(hp_f / PlayerMover.single.maxHealth, hp.rectTransform.localScale.y);
        hp2.rectTransform.localScale = new Vector3(hp.rectTransform.localScale.x > 0 ? 1 / (hp.rectTransform.localScale.x) : 0, hp2.rectTransform.localScale.y);
    }
    private void ChangeAmmo(float ammo_f, float cliprammo, float maxcliprammo)
    {
        ammo.text = ammo_f.ToString();
        ammoSpiner.fillAmount = Mathf.Clamp01(cliprammo / maxcliprammo);
    }
}
