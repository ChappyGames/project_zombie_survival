using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Server.Networking;
using ChappyGames.Server.Items;
using ChappyGames.Server.InventorySystem;

namespace ChappyGames.Server.Entities {
    public class Item : Entity {

        [Header("Item Properties")]
        [SerializeField] private string itemId;
        [SerializeField] private int stack;

        public string ItemId { get { return itemId; } }
        public int Stack { get { return stack; } }
        public ItemType ItemType { get { return ItemManager.Instance.GetItem(itemId).Type; } }

        public void Initialize(int aEntityId, Vector3 aPosition, string aItemId = null, int aStack = 1) {
            
            if (!string.IsNullOrEmpty(aItemId)) {
                itemId = aItemId;
            }

            if (stack <= 0) {
                stack = aStack;
            }

            base.Initialize("item", aEntityId, EntityType.ENTITY_ITEM);

            GameObject lWorldModel = Instantiate(ItemManager.Instance.GetItem(itemId).WorldModel);
            lWorldModel.transform.position = new Vector3(0f, 0f, 0f);
            lWorldModel.gameObject.transform.SetParent(this.transform, false);

        }

        protected override void FixedUpdate() {
            base.FixedUpdate();

            ServerSend.EntityPosition(this);
            ServerSend.EntityRotation(this);
        }

        public void Pickup(Mob aMob) {
            // Transfer this stack of items to the mob's inventory
            aMob.Inventory.AddItem(new InventoryItem(ItemType, itemId, stack));

            // Integrate unregistering entity logic.
        }

        #region Packets
        protected override Packet SpawnEntityPacket() {
            Packet lPacket = base.SpawnEntityPacket();

            lPacket.Write(itemId);
            lPacket.Write(stack);

            return lPacket;
        }
        #endregion
    }
}
