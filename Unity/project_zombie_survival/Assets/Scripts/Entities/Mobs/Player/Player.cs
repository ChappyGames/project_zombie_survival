using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.Items;

namespace ChappyGames.Client.Entities {

    public class Player : Mob {
        [Header("Player Properties")]
        public GameObject playerObj;
        public AudioSource playerAudioSource;
        public string username;

        public override Vector3 Position { get { return transform.position; } set { transform.position = value; } }
        public override Quaternion Rotation { get { return playerObj.transform.rotation; } set { playerObj.transform.rotation = value; } }

        public Weapon CurrentWeapon { get { return Inventory.PrimaryWeapon; } }

        public override void Initialize(int aId, EntityType lType, Packet aPacket) {
            base.Initialize(aId, EntityType.ENTITY_PLAYER, aPacket);

            username = aPacket.ReadString();
        }

        public void FireWeapon() {
            if (CurrentWeapon != null) {
                playerAudioSource.PlayOneShot(CurrentWeapon.FireSound);
            }
        }

        public void ReloadWeapon() {
            if (CurrentWeapon != null) {
                playerAudioSource.PlayOneShot(CurrentWeapon.ReloadSound);
            }
        }

        protected override void OnDie() {
            playerObj.SetActive(false);
        }

        public override void OnRespawn() {
            playerObj.SetActive(true);
            SetHealth(maxHealth);
        }
    }
}
