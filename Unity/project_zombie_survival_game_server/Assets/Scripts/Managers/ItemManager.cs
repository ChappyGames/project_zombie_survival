using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Server.Items {
    public class ItemManager : Singleton<ItemManager> {

        [SerializeField] private ItemDatabase itemDatabase;

        protected override void Awake() {
            base.Awake();

            itemDatabase.Initialize();
        }

        public Item GetItem(string aItemId) {
            Item lItem = null;
            lItem = itemDatabase.GetItem(aItemId);
            return lItem;
        }

        public Weapon GetWeapon(string aWeaponId) {
            Weapon lWeapon = null;
            lWeapon = itemDatabase.GetWeapon(aWeaponId);
            return lWeapon;
        }
    }
}
