using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class Zombie : Entity {

    public GameObject zombieObj;
    public AudioSource zombieAudioSource;

    public override void Initialize(int aId, EntityType aType) {
        base.Initialize(aId, aType);
    }

    protected override void OnDie() {
        base.OnDie();
        Debug.Log("Zombie Dying...");
        zombieObj.SetActive(false);
    }

    public override void OnRespawn() {
        base.OnRespawn();
        Debug.Log("Zombie Respawning...");
        zombieObj.SetActive(true);
        SetHealth(maxHealth);
    }
}
