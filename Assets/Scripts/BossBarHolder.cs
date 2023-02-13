using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NTC.Global.Cache;

[AddComponentMenu("Enemies/Boss Bar Handler")]
public class BossBarHolder : MonoCache
{
    public GameObject BossBarObj;
    public Image BossBarBG1;
    public Image BossBarBG2;
    public Image BossBarIm;
    public TextMeshProUGUI BossBarName;
}