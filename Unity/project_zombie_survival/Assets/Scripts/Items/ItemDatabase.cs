using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project Zombie Survival/New Item Database")]
public class ItemDatabase : ScriptableObject {
    [Header("Weapons")]
    [SerializeField] private List<Weapon> weapons;

    public List<Weapon> Weapons => weapons;

    [SerializeField] private Dictionary<string, Weapon> weaponsById;

    public void Initialize() {
        weaponsById = new Dictionary<string, Weapon>();

        for (int i = 0; i < weapons.Count; i++) {
            if (!weaponsById.ContainsKey(weapons[i].ID)) {
                weaponsById.Add(weapons[i].ID, weapons[i]);
            } else {
                Debug.LogError($"[Item Database] - ERROR: Database already contains weapon ID '{weapons[i].ID}'.");
            }
        }
    }

    public Weapon GetWeapon(string aWeaponId) {
        Weapon lWeapon = null;

        if (weaponsById.ContainsKey(aWeaponId) == true) {
            lWeapon = weaponsById[aWeaponId];
        } else {
            Debug.LogError($"[Item Database] - ERROR: Weapon ID '{aWeaponId}' not found.");
        }

        return lWeapon;
    }
}
