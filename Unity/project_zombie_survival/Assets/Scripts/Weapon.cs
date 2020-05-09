using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    NONE,
    PISTOL
}

public enum AmmoType {
    NONE,
    PISTOL_32
}

[CreateAssetMenu(menuName = "Dead Frontier Clone/New Weapon")]
public class Weapon : ScriptableObject {

    [SerializeField] private string weaponName;
    [TextArea(3, 5)]
    [SerializeField] private string weaponDescription;

    [SerializeField] private WeaponType weaponType;

    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int ammoCapacity;

    [SerializeField] private float damage;

    [SerializeField] private float reloadSpeed;
    [SerializeField] private float firingSpeed;
    [SerializeField] private float accuracy;
    [SerializeField] private float range;
    [SerializeField] private float criticalChance;

    [SerializeField] private int skillRequired;

    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;


    public WeaponType WeaponType { get { return weaponType; } }
    public AmmoType AmmoType { get { return ammoType; } }

    public int AmmoCapacity { get { return ammoCapacity; } }
    public float Damage { get { return damage; } }

    public float ReloadSpeed { get { return reloadSpeed; } }
    public float FiringSpeed { get { return firingSpeed; } }
    public float Accuracy { get { return accuracy; } }
    public float Range { get { return range; } }
    public float CriticalChance { get { return criticalChance; } }

    public AudioClip FireSound { get { return fireSound; } }
    public AudioClip ReloadSound { get { return reloadSound; } }
}
