using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.Items;

namespace ChappyGames.Client.Entities {

    public class Player : Mob {
        [Header("Player Properties")]
        public GameObject playerObj;
        public MeshRenderer mesh;
        public AudioSource playerAudioSource;
        public string username;
        public int clientId;

        public override Vector3 Position { get { return transform.position; } set { transform.position = value; } }
        public override Quaternion Rotation { get { return playerObj.transform.rotation; } set { playerObj.transform.rotation = value; } }

        public Weapon CurrentWeapon { get { return Inventory.PrimaryWeapon; } }

        public override void Initialize(Guid aId, EntityType lType, Packet aPacket) {
            base.Initialize(aId, EntityType.ENTITY_PLAYER, aPacket);

            username = aPacket.ReadString();
            clientId = aPacket.ReadInt();

            if (Networking.Client.instance.id == clientId) {
                mesh.material = GameManager.Instance.localPlayerMat;
                gameObject.AddComponent<PlayerController>().playerModel = playerObj;
                Camera.main.transform.SetParent(gameObject.transform);
            }

            GameManager.players.Add(clientId, this);
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

        protected override void OnDestroy() {
            base.OnDestroy();

            GameManager.players.Remove(clientId);
        }
    }
}
