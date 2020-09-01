using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager> {

    [SerializeField] private ItemDatabase itemDatabase;

    protected override void Awake() {
        base.Awake();

        itemDatabase.Initialize();
    }

    public Item GetItem(ItemType aType, string aItemId) {
        Item lItem = null;
        switch(aType) {
            case ItemType.ITEM_BASIC:
                lItem = itemDatabase.GetItem(aItemId);
                break;
            case ItemType.ITEM_WEAPON:
                lItem = itemDatabase.GetWeapon(aItemId);
                break;
        }

        return lItem;
    }
}
