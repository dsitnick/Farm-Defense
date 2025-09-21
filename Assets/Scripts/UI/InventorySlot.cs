using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image spriteImage;
    public TMP_Text countText;

    public void SetupSlot(PlayerInventory.SlotItem slotItem) {
        spriteImage.sprite = slotItem.item?.sprite;
        spriteImage.enabled = slotItem.item?.sprite != null;
        countText.text = slotItem.count > 1 ? slotItem.count.ToString() : "";

    }

    public void ClearSlot() {
        spriteImage.enabled = false;
        countText.text = "";
    }

}
