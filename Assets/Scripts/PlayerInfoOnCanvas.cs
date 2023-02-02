using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoOnCanvas : MonoBehaviour
{
    PlayerMover pm;
    [SerializeField] private Text hp;
    [SerializeField] private Text ammo;
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
            ammo.text = "Ammo = " + pm.weapon[pm.weaponSelect].ammo.ToString();
        }
    }
}
