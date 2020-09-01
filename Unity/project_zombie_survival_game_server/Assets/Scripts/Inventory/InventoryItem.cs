using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : IEquatable<InventoryItem> {
    public ItemType type;
    public string itemId;
    public int stack;

    public InventoryItem(ItemType aType, string aId, int aStack) {
        type = aType;
        itemId = aId;
        stack = aStack;
    }

    public InventoryItem(ItemType aType, string aId) {
        type = aType;
        itemId = aId;
        stack = 1;
    }

    public Item Item => ItemManager.Instance.GetItem(type, itemId);

    public override bool Equals(object obj) {
        return Equals(obj as InventoryItem);
    }

    public bool Equals(InventoryItem other) {
        return other != null &&
               type == other.type &&
               itemId == other.itemId;
    }

    public override int GetHashCode() {
        var hashCode = 371274160;
        hashCode = hashCode * -1521134295 + type.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemId);
        return hashCode;
    }
}
