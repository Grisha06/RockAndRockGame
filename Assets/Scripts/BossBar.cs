using UnityEngine;
using NTC.Global.Cache;

[AddComponentMenu("Enemies/Boss Bar")]
[RequireComponent(typeof(NewEnemyBace))]
public class BossBar : MonoCache
{
    [Min(0)]
    public float Tiling = 1.66f;
    [Min(0)]
    public float Distance;
    [Min(1f)]
    public float Width = 8.79f;
    public bool UseTiling;
    public bool use = true;
    public Color Color = Color.white;
    public Color BgColor = Color.red;
    [HideInInspector]
    public BossBarHolder bbh;
    [HideInInspector]
    public GameObject bbho;
    public GameObject BossBarPrefab;
    //[HideInInspector]
    public GameObject BossBarGroup;
    NewEnemyBace neb;
    private async void Start()
    {
        neb = GetComponent<NewEnemyBace>();

        BossBarGroup = GameObject.FindGameObjectWithTag("BossBarGroup");
        bbho = Instantiate(BossBarPrefab);
        bbho.transform.SetParent(BossBarGroup.transform);
        bbho.transform.localPosition = Vector3.zero;
        bbho.transform.localScale = Vector3.one;
        bbho.transform.rotation = Quaternion.identity;
        bbh = bbho.GetComponent<BossBarHolder>();
        bbho.SetActive(false);

        await System.Threading.Tasks.Task.Delay(500);

        UpdateBossBar(neb.hp);
        neb.OnHpChanged.AddListener(UpdateBossBar);
        PlayerMover.single.OnJumped.AddListener(() => { UpdateBossBar(neb.hp); });
    }

    public void UpdateBossBar(float hp)
    {
        if (use && (((neb ? neb.gameObject.CompareTag("Player") : false) ? true : Vector3.Distance(PlayerMover.single.tr.position, neb.tr.position) < Distance) || Distance == 0f))
        {
            bbho.SetActive(true);
            bbh.BossBarBG1.color = BgColor;
            bbh.BossBarBG2.color = BgColor;
            bbh.BossBarIm.color = Color;
            if (UseTiling)
            {
                bbh.BossBarBG2.gameObject.SetActive(true);
                bbh.BossBarBG2.pixelsPerUnitMultiplier = Tiling;
            }
            else
            {
                bbh.BossBarBG2.gameObject.SetActive(false);
            }
            bbh.BossBarIm.transform.localScale = new Vector3(hp > 0 ? (float)hp / (float)neb.maxHealth : 0, 1f, 1f);
            bbho.transform.localScale = new Vector3(Width, 1f, 1f);


            if (neb.EntityName != "/n")
            {
                bbh.BossBarName.gameObject.SetActive(true);
                bbh.BossBarName.text = neb.EntityName;
                bbh.BossBarName.rectTransform.localScale = new Vector3(1 / Width, 1f, 1f);
            }
            else
            {
                bbh.BossBarName.gameObject.SetActive(false);
            }
        }
        else
            bbho.SetActive(false);
    }
}


