using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public InventoryItem[] items = new InventoryItem[0];
    public int[] itemCounts = new int[0];
    
    void Start()
    {
        // Initialize item counts array to match items array
        if (itemCounts.Length != items.Length)
        {
            itemCounts = new int[items.Length];
        }
    }
    
    public void AddItem(InventoryItem item, int count = 1)
    {
        if (item == null) return;
        
        // Find existing item
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                itemCounts[i] += count;
                return;
            }
        }
        
        // Item not found, add new item
        System.Array.Resize(ref items, items.Length + 1);
        System.Array.Resize(ref itemCounts, itemCounts.Length + 1);
        
        items[items.Length - 1] = item;
        itemCounts[itemCounts.Length - 1] = count;
    }
    
    public int GetItemCount(InventoryItem item)
    {
        if (item == null) return 0;
        
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                return itemCounts[i];
            }
        }
        
        return 0;
    }
    
    public bool HasItem(InventoryItem item, int count = 1)
    {
        return GetItemCount(item) >= count;
    }
    
    public void RemoveItem(InventoryItem item, int count = 1)
    {
        if (item == null) return;
        
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                itemCounts[i] = Mathf.Max(0, itemCounts[i] - count);
                return;
            }
        }
    }
}
