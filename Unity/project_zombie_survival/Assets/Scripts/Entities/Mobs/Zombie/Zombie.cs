using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Client.Entities {

    public class Zombie : Mob {

        public GameObject zombieObj;
        public AudioSource zombieAudioSource;

        public override void Initialize(int aId, EntityType aType, Packet aPacket) {
            base.Initialize(aId, aType, aPacket);
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
}
