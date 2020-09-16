using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using ChappyGames.Entities;

public class Inventory {

    public class WeaponChangedEvent : UnityEvent<string> { }

    [SerializeField] private List<InventoryItem> items;

    private Mob parent;
    private string primaryWeaponId;

    public Weapon PrimaryWeapon { get { return ItemManager.Instance.GetItem(ItemType.ITEM_WEAPON, primaryWeaponId) as Weapon; } }

    public UnityEvent OnInventoryChanged { get; private set; } = new UnityEvent();
    public WeaponChangedEvent OnPrimaryWeaponChanged { get; private set; } = new WeaponChangedEvent();

    public Inventory(Mob lParent) {
        parent = lParent;
        items = new List<InventoryItem>();
    }

    public void SetWeapon(string aWeaponId) {
        InventoryItem lItem = items.Find((x) => x.itemId == aWeaponId);
        if (lItem != null) {
            primaryWeaponId = aWeaponId;
            OnPrimaryWeaponChanged?.Invoke(aWeaponId);
        }
    }

    public void AddItem(InventoryItem aItem) {

        InventoryItem lIdenticalItem = items.Find((x) => x.Equals(aItem));

        if (lIdenticalItem != null) {
            lIdenticalItem.stack += aItem.stack;
        }
        else {
            items.Add(aItem);
        }

        Debug.Log($"[Inventory] - Entity of type '{parent.Type}' with ID '{parent.ID}' has picked up {aItem.stack} instances of item with ID '{aItem.itemId}'.");
    }

    public void UseItem(InventoryItem aItem) {

        InventoryItem lItemInInventory = items.Find((x) => x.Equals(aItem));

        if (lItemInInventory != null) {
            lItemInInventory.Item.Use(parent);
            Debug.Log($"[Inventory] - Entity of type '{parent.Type}' with ID '{parent.ID}' has used item with ID '{aItem.itemId}'.");
        }
    }

    public void RemoveItem(InventoryItem aItem) {

        InventoryItem lIdenticalItem = items.Find((x) => x.Equals(aItem));

        if (lIdenticalItem != null) {

            if (lIdenticalItem.stack > aItem.stack) {
                lIdenticalItem.stack -= aItem.stack;
            }
            else {
                items.Remove(aItem);
            }

            Debug.Log($"[Inventory] - Entity of type '{parent.Type}' with ID '{parent.ID}' has removed {aItem.stack} instances of item with ID '{aItem.itemId}'.");
        }
    }
}
