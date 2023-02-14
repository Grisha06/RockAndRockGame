using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NTC.Global.Cache;

public class PlayerInfoOnCanvas : MonoCache
{
    PlayerMover pm;
    [SerializeField] private Text hp;
    [SerializeField] private Text ammo;
    static UnityEvent textUpdate;
    private void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
        StartCoroutine(enumerator());
    }
    IEnumerator enumerator()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.05f);
        while (true)
        {
            yield return wfs;
            hp.text = "HP = " + (pm.hp > 0 ? pm.hp : 0).ToString();
            ammo.text = "Ammo = " + pm.weapon[pm.weaponSelect].Ammo.ToString();
        }
    }
}
