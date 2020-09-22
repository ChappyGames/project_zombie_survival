using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerUIManager : Singleton<PlayerUIManager> {

    [SerializeField] private InventoryMenuController inventoryUI;
    [SerializeField] private PickupMenuController pickupUI;

    public PickupMenuController PickupUI => pickupUI;

    public void ToggleInventory() {
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
    }

}
