using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Entity {

    public GameObject playerObj;

    public AudioSource playerAudioSource;

    public string username;

    private string currentWeaponId;

    public override Transform Transform { get { return playerObj.transform; } }

    public Weapon CurrentWeapon { get { return ItemManager.Instance.GetWeapon(currentWeaponId); } }

    public void Initialize(int aId, string aUsername, string aWeaponId) {
        base.Initialize(aId, EntityType.ENTITY_PLAYER);

        username = aUsername;
        SetWeapon(aWeaponId);
    }

    public void SetWeapon(string aWeaponId) {
        currentWeaponId = aWeaponId;
    }

    public void FireWeapon() {
        playerAudioSource.PlayOneShot(CurrentWeapon.FireSound);
    }

    public void ReloadWeapon() {
        playerAudioSource.PlayOneShot(CurrentWeapon.ReloadSound);
    }

    protected override void OnDie() {
        playerObj.SetActive(false);
    }

    public override void OnRespawn() {
        playerObj.SetActive(true);
        SetHealth(maxHealth);
    }
}
