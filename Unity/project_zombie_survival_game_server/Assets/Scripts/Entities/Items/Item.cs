using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Server.Items;

namespace ChappyGames.Server.Entities {
    public class Item : Entity {

        [Header("Item Properties")]
        [SerializeField] private string itemId;
        [SerializeField] private uint stack;

        public void Initialize(int aEntityId, Vector3 aPosition, string aItemId = null) {
            base.Initialize(aEntityId, EntityType.ENTITY_ITEM);

            if (!string.IsNullOrEmpty(aItemId)) {
                itemId = aItemId;
            }

            GameObject lWorldModel = Instantiate(ItemManager.Instance.GetItem(itemId).WorldModel);
            lWorldModel.transform.position = aPosition;
            lWorldModel.gameObject.transform.SetParent(this.transform, false);
        }

        public void Pickup(Mob aMob) {
            // Transfer this stack of items to the mob's inventory
        }
    }
}
