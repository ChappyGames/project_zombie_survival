using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Entities;

public class EntityManager : Singleton<EntityManager> {

    [SerializeField] private EntityDatabase entityDatabase;

    private Dictionary<int, Dictionary<int, Entity>> entities;

    public EntityDatabase EntityDatabase => entityDatabase;

    protected override void Awake() {
        base.Awake();

        Initialize();
    }

    private void Initialize() {
        entityDatabase.Initialize();
        entities = new Dictionary<int, Dictionary<int, Entity>>();
    }

    public bool RegisterEntity(Entity aEntity) {
        bool lEntityAdded = false;

        if (!entities.ContainsKey((int)aEntity.Type)) {
            entities.Add((int)aEntity.Type, new Dictionary<int, Entity>());
        }

        if (!entities[(int)aEntity.Type].ContainsKey(aEntity.ID)) {
            entities[(int)aEntity.Type].Add(aEntity.ID, aEntity);
            lEntityAdded = true;
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

    public IEntity GetEntity(int aEntityType, int aEntityId) {
        IEntity lEntity = null;

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

}
