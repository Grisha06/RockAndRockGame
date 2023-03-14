using System;

namespace MyInventory
{
    public interface IInventoryItem
    {
        bool isEquipped { get; set; }
        Type type { get; }
        int maxItemsInInventorySlot { get; }
        int amount { get; set; }

        IInventoryItem Clone();
    }

    public interface IInventorySlot
    {
        bool isFull { get; }
        bool isEmpty { get; }

        IInventoryItem item { get; }
        Type itemType { get; }
        int amount { get; }
        int capacity { get; }

        void SetItem(IInventoryItem item);
        void Clear();
    }


    public interface IInventory
    {
        int capacity { get; set; }
        bool isFul { get; }

        IInventoryItem GetItem(Type itemType);
        IInventoryItem[] GetAllItems();
        IInventoryItem[] GetAllItems(Type itemType);
        IInventoryItem[] GetEquippedItems();
        int GetItemAmount(Type itemType);

        bool Add(object sender, IInventoryItem item);
        bool HasItem(object sender, out IInventoryItem item);
        bool Remove(object sender, IInventoryItem item, int amount = 1);

    }
}