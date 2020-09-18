using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Client.Entities {

    public class EntityManager : Singleton<EntityManager> {

        [SerializeField] private EntityDatabase entityDatabase;

        private Dictionary<int, Dictionary<int, Entity>> entities;

        public EntityDatabase EntityDatabase => entityDatabase;

        #region Life Cycle

        protected override void Awake() {
            base.Awake();

            Initialize();
        }

        private void Initialize() {
            entityDatabase.Initialize();
            entities = new Dictionary<int, Dictionary<int, Entity>>();
        }

        #endregion

        #region Register/UnRegister

        public bool RegisterEntity(Entity aEntity) {
            bool lEntityAdded = false;

            if (!entities.ContainsKey((int)aEntity.Type)) {
                entities.Add((int)aEntity.Type, new Dictionary<int, Entity>());
            }

            if (!entities[(int)aEntity.Type].ContainsKey(aEntity.ID)) {
                entities[(int)aEntity.Type].Add(aEntity.ID, aEntity);
                lEntityAdded = true;
                Debug.Log($"[Entity Manager] - Entity type '{aEntity.Type}' with ID '{aEntity.ID}' registered.");
            }
            else {
                Debug.LogError($"[Entity Manager] - Entity type '{aEntity.Type}' with ID '{aEntity.ID}' already exists.");
            }

            return lEntityAdded;
        }

        public bool UnregisterEntity(int aEntityType, int aEntityId) {
            bool lEntityRemoved = false;

            if (entities.ContainsKey(aEntityType)) {

                if (entities[aEntityType].ContainsKey(aEntityId)) {
                    entities[aEntityType].Remove(aEntityId);
                    lEntityRemoved = true;
                }
                else {
                    Debug.LogError($"[Entity Manager] - Entity type '{(EntityType)aEntityType}' with ID '{aEntityId}' does not exist.");
                }

            }
            else {
                Debug.LogError($"[Entity Manager] - Entity type '{(EntityType)aEntityType}' does not exist.");
            }

            return lEntityRemoved;
        }

        public bool UnregisterEntity(Entity aEntity) {
            return UnregisterEntity((int)aEntity.Type, aEntity.ID);
        }

        #endregion

        #region Get Entity

        public Entity GetEntity(int aEntityType, int aEntityId) {
            Entity lEntity = null;

            if (entities.ContainsKey(aEntityType)) {

                if (entities[aEntityType].ContainsKey(aEntityId)) {
                    lEntity = entities[aEntityType][aEntityId];
                }
                else {
                    Debug.LogError($"[Entity Manager] - Entity type '{(EntityType)aEntityType}' with ID '{aEntityId}' does not exist.");
                }

            }
            else {
                Debug.LogError($"[Entity Manager] - Entity type '{(EntityType)aEntityType}' does not exist.");
            }

            return lEntity;
        }

        // This is fucked. Refactor.
        public Mob GetMob(int aEntityType, int aEntityId) {
            return GetEntity(aEntityType, aEntityId) as Mob;
        }

        public Player GetPlayer(int aEntityType, int aEntityId) {
            return GetEntity(aEntityType, aEntityId) as Player;
        }

        #endregion

        public void SpawnEntityPacketHandler(Packet aPacket) {
            string lInstanceId = aPacket.ReadString();
            EntityType lType = (EntityType)aPacket.ReadInt();
            int lId = aPacket.ReadInt();
            Vector3 lPosition = aPacket.ReadVector3();
            Quaternion lRotation = aPacket.ReadQuaternion();

            GameObject lEntity = null;

            //Temporary switch case to handle the uniqueness of the player entity
            switch (lType) {
                case EntityType.ENTITY_PLAYER:
                    // Quick fix for the issue where the server sends two spawn player packets for the client.
                    if (GetEntity((int)EntityType.ENTITY_PLAYER, lId) != null) {
                        return;
                    }

                    if (lId == Networking.Client.instance.id) {
                        lEntity = Instantiate(entityDatabase.GetEntityData("player_local").EntityObject, lPosition, lRotation);
                    }
                    else {
                        lEntity = Instantiate(entityDatabase.GetEntityData("player_other").EntityObject, lPosition, lRotation);
                    }
                    break;
                default:
                    lEntity = Instantiate(entityDatabase.GetEntityData(lInstanceId).EntityObject, lPosition, lRotation);
                    break;
            }

            lEntity.GetComponent<Entity>().Initialize(lId, lType, aPacket);
        }
    }
}
