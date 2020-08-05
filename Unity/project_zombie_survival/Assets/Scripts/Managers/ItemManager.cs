using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager> {

    [SerializeField] private ItemDatabase itemDatabase;

    protected override void Awake() {
        base.Awake();

        itemDatabase.Initialize();
    }

    public Weapon GetWeapon(string aWeaponId) {
        return itemDatabase.GetWeapon(aWeaponId);
    }
}
