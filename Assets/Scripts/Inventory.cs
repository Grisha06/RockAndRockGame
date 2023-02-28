using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;
using UnityEngine.Events;

[RequireComponent(typeof(Entity))]
public class Inventory : MonoCache
{
    public List<InventoryItem> items;
    private Entity entity;
    public UnityEvent OnItemsChanged;
    private void Start()
    {
        entity = GetComponent<Entity>();
        entity.OnInvTriggerEntered.AddListener(AddItem);
    }
    private void AddItem(InventoryTrigger intr)
    {
        items.Add(intr.Item);
        OnItemsChanged.Invoke();
    }
    private void RemoveItem()
    {
        items.RemoveAt(items.Count - 1);
        OnItemsChanged.Invoke();
    }
}

[System.Serializable, SerializeField]
public struct InventoryItem
{
    public string Name;
    public string Description;
    public Sprite sprite;
}