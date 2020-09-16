using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Entities;

public enum WeaponType {
    NONE,
    PISTOL
}

public enum AmmoType {
    NONE,
    PISTOL_32
}

[CreateAssetMenu(menuName = "Project Zombie Survival/New Weapon")]
public class Weapon : Item {

    [Header("Weapon Properties")]

    [SerializeField] private WeaponType weaponType;

    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int ammoCapacity;

    [SerializeField] private int damage;

    [SerializeField] private float reloadSpeed;
    [SerializeField] private float firingSpeed;
    [SerializeField] private float accuracy;
    [SerializeField] private float range;
    [SerializeField] private float criticalChance;

    [SerializeField] private int skillRequired;

    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;

    public override ItemType Type { get { return ItemType.ITEM_WEAPON; } }
    public WeaponType WeaponType { get { return weaponType; } }
    public AmmoType AmmoType { get { return ammoType; } }

    public int AmmoCapacity { get { return ammoCapacity; } }
    public int Damage { get { return damage; } }

    public float ReloadSpeed { get { return reloadSpeed; } }
    public float FiringSpeed { get { return firingSpeed; } }
    public float Accuracy { get { return accuracy; } }
    public float Range { get { return range; } }
    public float CriticalChance { get { return criticalChance; } }

    public AudioClip FireSound { get { return fireSound; } }
    public AudioClip ReloadSound { get { return reloadSound; } }

    public override void Use(Mob aMob) {
        base.Use(aMob);

        aMob.Inventory.SetWeapon(new InventoryItem(ItemType.ITEM_WEAPON, ID));
    }
}
