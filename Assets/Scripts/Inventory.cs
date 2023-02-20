using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;

[RequireComponent(typeof(NewEnemyBace))]
public class Inventory : MonoCache
{
    [SerializeField]
    private List<InventoryItem> items;
    private NewEnemyBace entity;
    private void Start()
    {
        entity = GetComponent<NewEnemyBace>();
        items = new List<InventoryItem>();
        entity.OnInvTriggerEntered.AddListener(AddItem);
        entity.OnDie.AddListener(DropAll);
        entity.OnInventoryDrop.AddListener(RemoveItem);
    }
    private void AddItem(InventoryTrigger intr)
    {
        SpriteRenderer ssr=intr.gbj.GetComponent<SpriteRenderer>();
        ssr = ssr == null ? intr.gbj.GetComponentInChildren<SpriteRenderer>() : ssr;
        items.Add(new InventoryItem(intr.gbj, ssr));
    }
    private void RemoveItem()
    {
        GameObject g = Instantiate(items[items.Count - 1].obj);
        g.transform.localPosition = Vector3.zero;
        g.transform.rotation = Quaternion.identity;
        g.transform.SetParent(null);
        items.RemoveAt(items.Count - 1);
    }

    [ContextMenu("Drop all")]
    public void DropAll()
    {
        for (int i = 0; i < items.Count; i++)
        {
            RemoveItem();
        }
    }
}

[System.Serializable, SerializeField]
public struct InventoryItem
{
    public GameObject obj;
    public SpriteRenderer sr;
    public InventoryItem(GameObject ItemPrefab, SpriteRenderer ItemSpriteRenderer) : this()
    {
        obj = ItemPrefab;
        sr = ItemSpriteRenderer;
    }
}