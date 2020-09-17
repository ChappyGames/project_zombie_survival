using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using ChappyGames.Server.Entities;
using ChappyGames.Server.Items;
using ChappyGames.Server.Networking;

namespace ChappyGames.Server.InventorySystem {
    public class Inventory {

        public class WeaponChangedEvent : UnityEvent<string> { }

        [SerializeField] private List<InventoryItem> items;

        private Mob parent;
        private string primaryWeaponId;

        public Weapon PrimaryWeapon { get { return ItemManager.Instance.GetWeapon(primaryWeaponId); } }

        public UnityEvent OnInventoryChanged { get; private set; } = new UnityEvent();
        public WeaponChangedEvent OnPrimaryWeaponChanged { get; private set; } = new WeaponChangedEvent();

        public Inventory(Mob aParent) {
            parent = aParent;
            items = new List<InventoryItem>();
        }

        public void SetWeapon(InventoryItem aItem) {
            InventoryItem lItem = items.Find((x) => x.Equals(aItem));
            if (lItem != null) {
                primaryWeaponId = aItem.itemId;
                OnPrimaryWeaponChanged?.Invoke(aItem.itemId);
                Debug.Log($"[Inventory] - Entity of type '{parent.Type}' with ID '{parent.ID}' has equipped a weapon with ID '{aItem.itemId}'.");
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

            ServerSend.InventoryItemAdded(parent, aItem);
            Debug.Log($"[Inventory] - Entity of type '{parent.Type}' with ID '{parent.ID}' has picked up {aItem.stack} instances of item with ID '{aItem.itemId}'.");
        }

        public void UseItem(InventoryItem aItem) {

            InventoryItem lItemInInventory = items.Find((x) => x.Equals(aItem));

            if (lItemInInventory != null) {
                lItemInInventory.Item.Use(parent);
                ServerSend.InventoryItemUsed(parent, aItem);
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

                ServerSend.InventoryItemRemoved(parent, aItem);
                Debug.Log($"[Inventory] - Entity of type '{parent.Type}' with ID '{parent.ID}' has removed {aItem.stack} instances of item with ID '{aItem.itemId}'.");
            }
        }
    }
}
