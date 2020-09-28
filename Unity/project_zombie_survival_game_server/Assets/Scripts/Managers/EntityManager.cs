using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Server.Entities {

    public class EntityManager : Singleton<EntityManager> {

        [SerializeField] private EntityDatabase entityDatabase;

        private Dictionary<int, Dictionary<int, IEntity>> entities;

        public EntityDatabase EntityDatabase => entityDatabase;

        protected override void Awake() {
            base.Awake();

            Initialize();
        }

        private void Initialize() {
            entities = new Dictionary<int, Dictionary<int, IEntity>>();

            entityDatabase.Initialize();
        }

        public bool RegisterEntity(IEntity aEntity) {
            bool lEntityAdded = false;

            if (!entities.ContainsKey((int)aEntity.Type)) {
                entities.Add((int)aEntity.Type, new Dictionary<int, IEntity>());
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
                    Debug.LogError($"[Entity Manager] - Entity type '{aEntityType}' with ID '{aEntityId}' does not exist.");
                }

            }
            else {
                Debug.LogError($"[Entity Manager] - Entity type '{aEntityType}' does not exist.");
            }

            return lEntityRemoved;
        }

        public bool UnregisterEntity(IEntity aEntity) {
            return UnregisterEntity((int)aEntity.Type, aEntity.ID);
        }

        public IEntity GetEntity(int aEntityType, int aEntityId) {
            IEntity lEntity = null;

            if (entities.ContainsKey(aEntityType)) {

                if (entities[aEntityType].ContainsKey(aEntityId)) {
                    lEntity = entities[aEntityType][aEntityId];
                }
                else {
                    Debug.LogError($"[Entity Manager] - Entity type '{aEntityType}' with ID '{aEntityId}' does not exist.");
                }

            }
            else {
                Debug.LogError($"[Entity Manager] - Entity type '{aEntityType}' does not exist.");
            }

            return lEntity;
        }

        public int GetEntityCount(int aEntityType) {
            int lCount = 0;

            if (entities.ContainsKey(aEntityType)) {
                lCount = entities[aEntityType].Count;
            }

            return lCount;
        }

        public void InvokeOnAllEntities(Action<IEntity> aAction) {
            foreach (KeyValuePair<int, Dictionary<int, IEntity>> lEntityType in entities) {
                foreach (IEntity lEntity in lEntityType.Value.Values) {
                    aAction.Invoke(lEntity);
                }
            }
        }
    }
}
