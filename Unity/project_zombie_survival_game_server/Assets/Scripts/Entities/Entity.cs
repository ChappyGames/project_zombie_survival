using System;
using UnityEngine;

using ChappyGames.Server.Networking;

namespace ChappyGames.Server.Entities {

    public enum EntityType {
        ENTITY_NONE = 0,
        ENTITY_PLAYER = 1,
        ENTITY_ZOMBIE = 2,
        ENTITY_ITEM = 3,
    }

    public interface IEntity {
        Guid ID { get; }
        EntityType Type { get; }

        void Initialize(string aInstanceId, EntityType aType);

        void ServerSpawnEntity(int aToClient);
    }
    public class Entity : MonoBehaviour, IEntity {

        protected string instanceId;

        protected Guid id;
        protected EntityType type;

        public string InstanceId { get { return instanceId; } }
        public Guid ID { get { return id; } }
        public EntityType Type { get { return type; } }

        public virtual void Initialize(string aInstanceId, EntityType aType) {
            instanceId = aInstanceId;
            type = aType;

            id = Guid.NewGuid();

            if (EntityManager.Instance.RegisterEntity(this)) {
                SpawnToAllExistingPlayers();
            } else {
                Debug.LogError($"[Entity] - Entity '{type}' with ID '{id}' failed to initialize properly. Destroying associated Game Object.");
            }
        }

        protected virtual void SpawnToAllExistingPlayers() {
            foreach (Client lClient in Networking.Server.clients.Values) {
                if (lClient.player != null) {
                    ServerSpawnEntity(lClient.id);
                }
            }
        }

        public virtual void ServerSpawnEntity(int aToClient) {
            //ServerSend.SpawnEntity(aToClient, this);
            // Consider wrapping packets in a Using statement to properly dispose after this method concludes.
            ServerSend.SendTCPData(aToClient, SpawnEntityPacket());
        }

        public virtual void ServerDespawnEntity() {
            ServerSend.SendTCPDataToAll(DespawnEntityPacket());
        }

        protected virtual void FixedUpdate() {
            // NO OP
        }

        protected virtual void OnDestroy() {
            if (EntityManager.Instance.UnregisterEntity(this)) {
                ServerDespawnEntity();
            }
        }

        #region Packets
        protected virtual Packet SpawnEntityPacket() {
            Packet lPacket = new Packet((int)ServerPackets.ENTITY_SPAWN);

            lPacket.Write(instanceId);
            lPacket.Write((int)type);
            lPacket.Write(id);
            lPacket.Write(transform.position);
            lPacket.Write(transform.rotation);

            return lPacket;
        }

        protected virtual Packet DespawnEntityPacket() {
            Packet lPacket = new Packet((int)ServerPackets.ENTITY_DESPAWN);

            lPacket.Write((int)type);
            lPacket.Write(id);

            return lPacket;
        }
        #endregion
    }
}
