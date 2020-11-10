using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Client.Entities {

    public class EntityManager : Singleton<EntityManager> {

        [SerializeField] private EntityDatabase entityDatabase;

        private Dictionary<int, Dictionary<Guid, Entity>> entities;

        public EntityDatabase EntityDatabase => entityDatabase;

        #region Life Cycle

        protected override void Awake() {
            base.Awake();

            Initialize();
        }

        private void Initialize() {
            entityDatabase.Initialize();
            entities = new Dictionary<int, Dictionary<Guid, Entity>>();
        }

        #endregion

        #region Register/UnRegister

        public bool RegisterEntity(Entity aEntity) {
            bool lEntityAdded = false;

            if (!entities.ContainsKey((int)aEntity.Type)) {
                entities.Add((int)aEntity.Type, new Dictionary<Guid, Entity>());
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

        public bool UnregisterEntity(int aEntityType, Guid aEntityId) {
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

        public Entity GetEntity(int aEntityType, Guid aEntityId) {
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
        public Mob GetMob(int aEntityType, Guid aEntityId) {
            return GetEntity(aEntityType, aEntityId) as Mob;
        }

        public Player GetPlayer(int aEntityType, Guid aEntityId) {
            return GetEntity(aEntityType, aEntityId) as Player;
        }

        #endregion

        public void SpawnEntityPacketHandler(Packet aPacket) {
            string lInstanceId = aPacket.ReadString();
            EntityType lType = (EntityType)aPacket.ReadInt();
            Guid lId = aPacket.ReadGuid();
            Vector3 lPosition = aPacket.ReadVector3();
            Quaternion lRotation = aPacket.ReadQuaternion();

            if (!GetEntity((int)lType, lId)) {
                GameObject lEntity = Instantiate(entityDatabase.GetEntityData(lInstanceId).EntityObject, lPosition, lRotation);
                lEntity.GetComponent<Entity>().Initialize(lId, lType, aPacket);
            } else {
                Debug.LogError($"[Entity Manager] - Failed to spawn Entity of type '{lType}' with ID '{lId}'. Entity already exists. Perhaps the existing Entity failed to despawn properly?");
            }
        }
    }
}
