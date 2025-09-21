using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class PlayerInventory : MonoBehaviour {

    [System.Serializable]
    public struct SlotItem {
        public InventoryItem item;
        public int count;
    }

    public SlotItem[] inventoryItems = new SlotItem[INVENTORY_SIZE];

    public UnityEvent onUpdateInventory;

    public const int INVENTORY_SIZE = 32;
    public const int NUM_TOOLS = 8;

    private PlayerTools playerTools;

    void Awake() {
        playerTools = GetComponent<PlayerTools>();
    }

    void Start() {
        for (int i = 0; i < NUM_TOOLS; i++) {
            UpdateToolAt(i);
        }

        onUpdateInventory.Invoke();
    }

    void OnValidate() {
        if (inventoryItems.Length != INVENTORY_SIZE) {
            Array.Resize(ref inventoryItems, INVENTORY_SIZE);
        }
    }

    public void AddItem(InventoryItem item, int count = 1) {
        if (item == null) return;

        int slotIndex = GetSlotIndex(item);
        if (slotIndex < 0) {
            slotIndex = GetFirstEmptySlot();

            if (slotIndex < 0) return;// Inventory is full

            inventoryItems[slotIndex] = new SlotItem { item = item, count = count };

            if (slotIndex < NUM_TOOLS) {
                UpdateToolAt(slotIndex);
            }
        }
        else {
            inventoryItems[slotIndex].count += count;
        }

        onUpdateInventory.Invoke();
    }

    public void RemoveItem(InventoryItem item, int count = 1) {
        if (item == null) return;

        int slotIndex = GetSlotIndex(item);
        if (slotIndex < 0) return;

        inventoryItems[slotIndex].count -= count;
        if (inventoryItems[slotIndex].count <= 0) {
            inventoryItems[slotIndex] = default;

            UpdateToolAt(slotIndex);
        }

        onUpdateInventory.Invoke();
    }

    public void SwapSlots(int slotA, int slotB) {
        SlotItem itemA = inventoryItems[slotA];
        SlotItem itemB = inventoryItems[slotB];

        inventoryItems[slotA] = itemB;
        inventoryItems[slotB] = itemA;

        if (slotA < NUM_TOOLS) UpdateToolAt(slotA);
        if (slotB < NUM_TOOLS) UpdateToolAt(slotB);
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

    private void UpdateToolAt(int toolIndex) {
        playerTools.SetTool(toolIndex, inventoryItems[toolIndex].item?.toolPrefab);
    }

    private int GetSlotIndex(InventoryItem item) {
        for (int i = 0; i < inventoryItems.Length; i++) {
            if (inventoryItems[i].item == item) return i;
        }
        return -1;
    }

    private int GetFirstEmptySlot() {
        for (int i = 0; i < INVENTORY_SIZE; i++) {
            if (inventoryItems[i].item == null) return i;
        }
        return -1;
    }
}
