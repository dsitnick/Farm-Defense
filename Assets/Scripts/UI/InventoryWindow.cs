using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : MonoBehaviour {

    public PlayerInventory playerInventory;

    public InventorySlot[] inventorySlots;

    int pageIndex = 0;
    int requiredPages => Mathf.Max(1, Mathf.CeilToInt((float)playerInventory.inventoryItems.Count / (float)SLOTS_PER_PAGE));


    const int SLOTS_PER_PAGE = 32;

    public void Back() {
        if (pageIndex <= 0) return;

        UpdateSlots();
    }

    public void Forward() {
        if (pageIndex >= requiredPages) return;
        pageIndex++;

        UpdateSlots();
    }

    public void Refresh() {
        if (pageIndex > requiredPages) {
            pageIndex = requiredPages;
        }
        UpdateSlots();
    }

    void UpdateSlots() {
        for (int i = 0; i < SLOTS_PER_PAGE; i++) {
            if (i < playerInventory.inventoryItems.Count) {
                inventorySlots[i].SetupSlot(playerInventory.inventoryItems[pageIndex * SLOTS_PER_PAGE + i]);
            }
            else {
                inventorySlots[i].ClearSlot();
            }

        }

    }

}
