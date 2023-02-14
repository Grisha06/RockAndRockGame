using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NTC.Global.Cache;
using UnityEngine.Events;

public class PlayerInfoOnCanvasEvent : UnityEvent<float, float> { }

public class PlayerInfoOnCanvas : MonoCache
{
    public static PlayerInfoOnCanvasEvent textUpdate;
    [SerializeField] private Text hp;
    [SerializeField] private Text ammo;
    private void Start()
    {
        textUpdate = new PlayerInfoOnCanvasEvent();
        textUpdate.AddListener((float hp_f, float ammo_f) => {
            hp.text = "HP = " + (hp_f > 0 ? hp_f : 0).ToString();
            ammo.text = "Ammo = " + ammo_f.ToString();
        });
    }
}
