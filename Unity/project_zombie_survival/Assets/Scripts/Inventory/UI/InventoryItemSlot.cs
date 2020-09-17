using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ChappyGames.Client.InventorySystem;
using ChappyGames.Client.Networking;

public class InventoryItemSlot : MonoBehaviour {

    public Text itemNameText;

    private InventoryItem item;

    public InventoryItem Item { get { return item; } }

    public void Initialize(InventoryItem aItem) {
        item = aItem;

        itemNameText.text = item.stack > 1 ? item.Item.ItemName + " (" + item.stack + ")" : item.Item.ItemName;
    }

    public void Use() {
        ClientSend.PlayerItemUsed(item);
    }
}
