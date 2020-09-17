using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.Networking;
using ChappyGames.Client.Entities;
using ChappyGames.Client.InventorySystem;

public class InventoryMenuController : MonoBehaviour {

    [SerializeField] private InventoryItemSlot slotPrefab;
    [SerializeField] private GameObject content;

    private SortedList<string, InventoryItemSlot> slots = new SortedList<string, InventoryItemSlot>();

    private Inventory ParentInventory { get { return EntityManager.Instance.GetMob((int)EntityType.ENTITY_PLAYER, Client.instance.id).Inventory; } }

    private void OnEnable() {
        ParentInventory.OnInventoryChanged.AddListener(Refresh);
        Refresh();
    }

    private void OnDisable() {
        ParentInventory.OnInventoryChanged.RemoveListener(Refresh);
    }

    private void Refresh() {
        // This will work for now lol.
        foreach (KeyValuePair<string, InventoryItemSlot> lEntry in slots) {
            Destroy(lEntry.Value.gameObject);
        }
        slots.Clear();

        // Add inventory slots to a sorted list.
        for (int i = 0; i < ParentInventory.Count; i++) {
            InventoryItemSlot lNewSlot = Instantiate(slotPrefab);
            lNewSlot.transform.SetParent(content.transform, false);
            lNewSlot.Initialize(ParentInventory.GetItem(i));

            slots.Add(lNewSlot.Item.Item.ItemName, lNewSlot);
        }

        // Sort the items in the hierarchy to be identical with the sorted list
        for (int i = 0; i < slots.Count; i++) {
            slots.Values[i].transform.SetSiblingIndex(i);
        }
    }

}
