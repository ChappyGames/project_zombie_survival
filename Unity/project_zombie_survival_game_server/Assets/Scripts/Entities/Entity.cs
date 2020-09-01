using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChappyGames.Networking;

namespace ChappyGames.Entities {

    public enum EntityType {
        ENTITY_NONE = 0,
        ENTITY_PLAYER = 1,
        ENTITY_ZOMBIE = 2,
        ENTITY_ITEM = 3,
    }

    public interface IEntity {
        int ID { get; }
        EntityType Type { get; }

        void ServerSpawnEntity(int aToClient);
    }
    public class Entity : MonoBehaviour, IEntity {

        protected int id;
        protected EntityType type;

        public int ID { get { return id; } }
        public EntityType Type { get { return type; } }


        public virtual void Initialize(int aId, EntityType aType) {
            id = aId;
            type = aType;

            EntityManager.Instance.RegisterEntity(this);
            SpawnToAllExistingPlayers();
        }

        protected virtual void SpawnToAllExistingPlayers() {
            foreach (Client lClient in Server.clients.Values) {
                if (lClient.player != null) {
                    ServerSpawnEntity(lClient.id);
                }
            }
        }

        public virtual void ServerSpawnEntity(int aToClient) {
            ServerSend.SpawnEntity(aToClient, this);
        }


        protected virtual void FixedUpdate() {
            // NO OP
        }

        protected virtual void OnDestroy() {
            EntityManager.Instance.UnregisterEntity(this);
        }
    }
}
