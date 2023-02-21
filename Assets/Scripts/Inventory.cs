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
        entity.OnInvTriggerEntered.AddListener(AddItem);
        entity.OnDie.AddListener(DropAll);
        entity.OnInventoryDrop.AddListener(RemoveItem);
    }
    private void AddItem(InventoryTrigger intr)
    {
        SpriteRenderer ssr = intr.GetComponent<SpriteRenderer>();
        ssr = ssr == null ? intr.GetComponentInChildren<SpriteRenderer>() : ssr;
        items.Add(new InventoryItem(intr.gameObject, ssr));
    }
    private void RemoveItem()
    {
        if (items.Count != 0)
        {
            items[items.Count - 1].obj.SetActive(true);
            items[items.Count - 1].obj.transform.position = entity.tr.position + new Vector3(1, 0);
            items[items.Count - 1].obj.transform.rotation = Quaternion.identity;
            items.RemoveAt(items.Count - 1);
        }
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