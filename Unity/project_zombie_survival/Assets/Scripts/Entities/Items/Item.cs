using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.Items;

namespace ChappyGames.Client.Entities {
    public class Item : Entity {

        [Header("Item Properties")]
        [SerializeField] private string itemId;
        [SerializeField] private int stack;

        public string ItemId => itemId;
        public int Stack => stack;

        public override void Initialize(Guid aId, EntityType aType, Packet aPacket) {
            base.Initialize(aId, aType, aPacket);

            itemId = aPacket.ReadString();
            stack = aPacket.ReadInt();

            GameObject lWorldModel = Instantiate(ItemManager.Instance.GetItem(itemId).WorldModel);
            lWorldModel.gameObject.transform.SetParent(this.transform, false);
        }
    }
}
