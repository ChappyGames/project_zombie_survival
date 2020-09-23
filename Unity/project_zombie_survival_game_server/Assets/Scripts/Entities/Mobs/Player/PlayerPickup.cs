using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Server.InventorySystem;

namespace ChappyGames.Server.Entities {

    public class PlayerPickup : MonoBehaviour {

        private Player parent;

        private List<Item> pickupsInRange = new List<Item>();

        public void Initialize(Player aParent) {
            parent = aParent;
        }

        public void Pickup() {
            if (pickupsInRange.Count > 0) {
                pickupsInRange[0].Pickup(parent);
                pickupsInRange.RemoveAt(0);
                Refresh();
            }
        }

        private void OnTriggerEnter(Collider other) {
            Item lItem = other.gameObject.GetComponentInParent<Item>();

            if (lItem != null) {
                Debug.Log($"[Player Pickup] - Item '{lItem.ItemId}' is within range of Player '{parent.username}'.");
                pickupsInRange.Add(lItem);

                // Only send a packet to the client if this is the first item within range.
                if (pickupsInRange.Count == 1) {
                    Networking.ServerSend.PlayerItemInRange(parent.ID, lItem);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            Item lItem = other.gameObject.GetComponentInParent<Item>();

            if (lItem != null) {
                Debug.Log($"[Player Pickup] - Item '{lItem.ItemId}' is outside the range of Player '{parent.username}'.");
                //bool lNewItemPickup = pickupsInRange[0] == lItem;
                pickupsInRange.Remove(lItem);
                Refresh();
            }
        }

        private void Refresh() {
            if (pickupsInRange.Count > 0) {
                Networking.ServerSend.PlayerItemInRange(parent.ID, pickupsInRange[0]);
            }
            else if (pickupsInRange.Count == 0) {
                Networking.ServerSend.PlayerItemOutRange(parent.ID);
            }
        }
    }
}
