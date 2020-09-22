using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Server.Networking;
using ChappyGames.Server.InventorySystem;

namespace ChappyGames.Server.Entities {
    public class Player : Mob {
        [Header("Player Properties")]
        public string username;
        public PlayerAttack attack;
        public PlayerPickup pickup;

        private bool[] inputs;

        public void Initialize(int aId, string aUsername) {
            
            username = aUsername;
            inputs = new bool[4];

            base.Initialize("player", aId, EntityType.ENTITY_PLAYER);

            attack.Initialize(this);
            pickup.Initialize(this);
            SpawnAllEntitiesToPlayerClient();
        }

        private void SpawnAllEntitiesToPlayerClient() {
            //TODO: This spawns all server entities to the client INCLUDING the client entity itself. We should skip that one instance.
            EntityManager.Instance.InvokeOnAllEntities((lEntity) => { lEntity.ServerSpawnEntity(id); });
        }

        /*public override void ServerSpawnEntity(int aToClient) {
            ServerSend.SpawnPlayer(aToClient, this);
        }*/

        protected override void FixedUpdate() {
            base.FixedUpdate();

            if (health <= 0f) {
                return;
            }

            Vector2 lInputDirection = Vector2.zero;
            if (inputs[0]) {
                lInputDirection.y += 1;
            }
            if (inputs[1]) {
                lInputDirection.y -= 1;
            }
            if (inputs[2]) {
                lInputDirection.x -= 1;
            }
            if (inputs[3]) {
                lInputDirection.x += 1;
            }

            Move(lInputDirection);
        }

        private void Move(Vector2 aInputDirection) {

            Vector3 lMoveDirection = new Vector3(1, 0, 0) * aInputDirection.x + new Vector3(0, 0, 1) * aInputDirection.y;
            transform.position += lMoveDirection * MoveSpeed;

            ServerSend.EntityPosition(this);
            ServerSend.EntityRotation(this);
        }

        public void SetInput(bool[] aInput, Quaternion aRotation) {
            inputs = aInput;
            transform.rotation = aRotation;
        }

        private void OnPrimaryWeaponEquipped(string aWeaponId) {
            Debug.Log("Sending weapon equipped packet!");
            ServerSend.WeaponEquipped(this);
        }

        #region Packets
        protected override Packet SpawnEntityPacket() {
            Packet lPacket = base.SpawnEntityPacket();

            lPacket.Write(username);

            return lPacket;
        }
        #endregion
    }
}
