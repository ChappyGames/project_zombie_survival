using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject playerObj;

    public int id;
    public string username;

    public float health;
    public float maxHealth;

    public Transform ModelTransform { get { return playerObj.transform; } }

    public void Initialize(int aId, string aUsername) {
        id = aId;
        username = aUsername;

        health = maxHealth;
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
