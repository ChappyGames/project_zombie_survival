using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory {

    public class WeaponChangedEvent : UnityEvent<string> { }

    [SerializeField] private List<InventoryItem> items;

    private string primaryWeaponId;

    public Weapon PrimaryWeapon { get { return ItemManager.Instance.GetItem(ItemType.ITEM_WEAPON, primaryWeaponId) as Weapon; } }

    public UnityEvent OnInventoryChanged { get; private set; } = new UnityEvent();
    public WeaponChangedEvent OnPrimaryWeaponChanged { get; private set; } = new WeaponChangedEvent();

    public Inventory() {
        items = new List<InventoryItem>();
    }

    public void SetWeapon(string aWeaponId) {
        //Temp function
        primaryWeaponId = aWeaponId;
        OnPrimaryWeaponChanged?.Invoke(aWeaponId);
    }

    public void AddItem(InventoryItem aItem) {

        InventoryItem lIdenticalItem = items.Find((x) => x.Equals(aItem));

        if (lIdenticalItem != null) {
            lIdenticalItem.stack += aItem.stack;
        } else {
            items.Add(aItem);
        }
    }

    public void RemoveItem(InventoryItem aItem) {

        InventoryItem lIdenticalItem = items.Find((x) => x.Equals(aItem));

        if (lIdenticalItem != null) {

            if (lIdenticalItem.stack > aItem.stack) {
                lIdenticalItem.stack -= aItem.stack;
            } else {
                items.Remove(aItem);
            }
        }
    }
}
