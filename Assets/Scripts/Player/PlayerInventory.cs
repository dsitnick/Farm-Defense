using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour {

    [System.Serializable]
    public class SlotItem {
        public InventoryItem item;
        public int count;
        public int toolSlot = 0;
    }

    [Header("Inventory Settings")]
    public List<SlotItem> inventoryItems = new();

    public UnityEvent onUpdateInventory;

    void Start() {
        foreach (SlotItem item in inventoryItems) {
            if (item.toolSlot > 0) {
                SetItemToolSlot(item.item, item.toolSlot);
            }
        }
        onUpdateInventory.Invoke();
    }

    public void AddItem(InventoryItem item, int count = 1) {
        if (item == null) return;

        int slotIndex = GetSlotIndex(item);
        if (slotIndex < 0) {
            inventoryItems.Add(new SlotItem { item = item, count = count });
            int toolSlot = GetComponent<PlayerTools>().GetAvailableSlot() + 1;
            if (toolSlot > 0) {
                SetItemToolSlot(item, toolSlot);
            }
        }
        else {
            inventoryItems[slotIndex].count += count;
        }

        onUpdateInventory.Invoke();
    }

    public int GetItemCount(InventoryItem item) {
        if (item == null) return 0;

        int slotIndex = GetSlotIndex(item);
        if (slotIndex < 0) return 0;

        return inventoryItems[slotIndex].count;
    }

    public bool HasItem(InventoryItem item, int count = 1) {
        return GetItemCount(item) >= count;
    }

    public void RemoveItem(InventoryItem item, int count = 1) {
        if (item == null) return;

        int slotIndex = GetSlotIndex(item);
        if (slotIndex < 0) return;

        inventoryItems[slotIndex].count -= count;
        if (inventoryItems[slotIndex].count <= 0) {
            inventoryItems.RemoveAt(slotIndex);
        }

        onUpdateInventory.Invoke();
    }

    public void SetItemToolSlot(InventoryItem item, int toolSlot) {
        if (item == null) return;

        int slotIndex = GetSlotIndex(item);
        if (slotIndex < 0) return;

        inventoryItems[slotIndex].toolSlot = toolSlot;
        GetComponent<PlayerTools>().SetTool(toolSlot, item.toolPrefab);

        onUpdateInventory.Invoke();

    }

    private int GetSlotIndex(InventoryItem item) {
        for (int i = 0; i < inventoryItems.Count; i++) {
            if (inventoryItems[i].item == item) return i;
        }
        return -1;
    }
}
