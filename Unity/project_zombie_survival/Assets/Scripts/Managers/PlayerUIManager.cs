using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerUIManager : Singleton<PlayerUIManager> {

    [SerializeField] private InventoryMenuController inventoryUI;

    public void ToggleInventory() {
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
    }

}
