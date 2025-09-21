using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public InventoryWindow inventoryWindow;

    void Start() {
        inventoryWindow.SetWindowActive(false);
        SetMenu(null);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ToggleMenu(inventoryWindow);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetMenu(null);
        }
    }

    IMenu lastMenu = null;
    void SetMenu(IMenu menu) {
        SetMovementDisabled(menu != null);

        if (lastMenu != null) lastMenu.SetWindowActive(false);
        if (menu != null) menu.SetWindowActive(true);

        lastMenu = menu;

    }

    void ToggleMenu(IMenu menu) {
        if (lastMenu == menu) {
            SetMenu(null);
        }
        else {
            SetMenu(menu);
        }
    }

    void SetMovementDisabled(bool isDisabled) {
        PlayerMovement.MOVEMENT_DISABLED = isDisabled;
        if (isDisabled) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

}

public interface IMenu {

    void SetWindowActive(bool isActive);

}
