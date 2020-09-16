using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Entities {
    public class Item : Entity {

        [Header("Item Properties")]
        [SerializeField] private string itemId;
        [SerializeField] private uint stack;

        public void Initialize(int aEntityId, string aItemId = null) {
            base.Initialize(aEntityId, EntityType.ENTITY_ITEM);

            if (!string.IsNullOrEmpty(aItemId)) {
                itemId = aItemId;
            }
        }

        public void Pickup(Mob aMob) {
            // Transfer this stack of items to the mob's inventory
        }
    }
}
