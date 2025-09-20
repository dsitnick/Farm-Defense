using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image spriteImage;
    public TMP_Text countText, slotText;
    public GameObject slotOverlay;

    public void SetupSlot(PlayerInventory.SlotItem slotItem) {
        spriteImage.sprite = slotItem.item.sprite;
        spriteImage.enabled = slotItem.item.sprite != null;
        countText.text = slotItem.count > 1 ? slotItem.count.ToString() : "";
        slotText.text = slotItem.toolSlot.ToString();
        slotOverlay.SetActive(slotItem.toolSlot >= 0);

    }

    public void ClearSlot() {
        spriteImage.enabled = false;
        countText.text = "";
        slotOverlay.SetActive(false);
    }

}
