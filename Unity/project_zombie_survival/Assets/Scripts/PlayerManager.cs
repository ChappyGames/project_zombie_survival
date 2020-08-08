using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Entity {

    public GameObject playerObj;

    public AudioSource playerAudioSource;

    public string username;

    private string currentWeaponId;

    public override Vector3 Position { get { return transform.position; } set { transform.position = value; } }
    public override Quaternion Rotation { get { return playerObj.transform.rotation; } set { playerObj.transform.rotation = value; } }

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
