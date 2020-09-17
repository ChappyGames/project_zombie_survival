using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Server.Items {

    [CreateAssetMenu(menuName = "Project Zombie Survival/Items/New Item Database")]
    public class ItemDatabase : ScriptableObject {
        [Header("Items")]
        [SerializeField] private List<Item> items;
        [Header("Weapons")]
        [SerializeField] private List<Weapon> weapons;

        public List<Item> Items => items;
        public List<Weapon> Weapons => weapons;

        private IdSystem<string, Item> itemsById;

        public void Initialize() {
            itemsById = new IdSystem<string, Item>();

            for (int i = 0; i < items.Count; i++) {
                if (!itemsById.Register(items[i].ID, items[i])) {
                    Debug.LogError($"[Item Database] - ERROR: Database already contains item ID '{weapons[i].ID}'.");
                }
            }

            for (int i = 0; i < weapons.Count; i++) {
                if (!itemsById.Register(weapons[i].ID, weapons[i])) {
                    Debug.LogError($"[Item Database] - ERROR: Database already contains weapon ID '{weapons[i].ID}'.");
                }
            }
        }

        public Item GetItem(string aItemId) {
            return itemsById.GetValue(aItemId);
        }
        public Weapon GetWeapon(string aWeaponId) {
            return itemsById.GetValue(aWeaponId) as Weapon;
        }
    }
}
