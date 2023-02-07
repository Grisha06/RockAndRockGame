using UnityEngine;

[AddComponentMenu("Enemies/Boss Bar")]
[RequireComponent(typeof(NewEnemyBace))]
public class BossBar : MonoBehaviour
{
    [Min(0)]
    public float Tiling = 1.66f;
    [Min(0)]
    public float Distance;
    public float Width= 8.79f;
    public bool UseTiling;
    [HideInInspector]
    public bool use;
    public Color Color = Color.white;
    public Color BgColor = Color.red;
    [HideInInspector]
    public BossBarHolder bbh;
    [HideInInspector]
    public GameObject bbho;
    public GameObject BossBarPrefab;
    public GameObject BossBarGroup;
    NewEnemyBace neb;
    private void Start()
    {
        neb = GetComponent<NewEnemyBace>();

        bbho = Instantiate(BossBarPrefab);
        bbho.transform.SetParent(BossBarGroup.transform);
        bbho.transform.localPosition = Vector3.zero;
        bbho.transform.localScale = Vector3.one;
        bbho.transform.rotation = Quaternion.identity;

        bbh = bbho.GetComponent<BossBarHolder>();
    }
    private void LateUpdate()
    {
        if (use && (Vector3.Distance(neb.pl.transform.position, transform.position) < Distance || Distance == 0f))
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
            bbh.BossBarIm.transform.localScale = new Vector3(Mathf.Clamp01(1f - (1f / (float)neb.hp)), 1f, 1f);
            bbho.transform.localScale = new Vector3(Width, 1f, 1f);
        }
        else
            bbho.SetActive(false);
    }
}


