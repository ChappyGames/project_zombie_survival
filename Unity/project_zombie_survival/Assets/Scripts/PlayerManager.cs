using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject playerObj;

    public AudioSource playerAudioSource;

    public int id;
    public string username;

    public float health;
    public float maxHealth;

    private string currentWeaponId;

    public Transform ModelTransform { get { return playerObj.transform; } }

    public Weapon CurrentWeapon { get { return ItemManager.Instance.GetWeapon(currentWeaponId); } }

    public void Initialize(int aId, string aUsername, string aWeaponId) {
        id = aId;
        username = aUsername;
        SetWeapon(aWeaponId);

        health = maxHealth;
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

    public void SetHealth(float aHealth) {
        health = aHealth;

        if (health <= 0f) {
            Die();
        }
    }

    public void Die() {
        playerObj.SetActive(false);
    }

    public void Respawn() {
        playerObj.SetActive(true);
        SetHealth(maxHealth);
    }
}
