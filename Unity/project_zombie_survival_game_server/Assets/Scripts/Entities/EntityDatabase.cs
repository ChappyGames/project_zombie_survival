using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Server.Entities {
    [CreateAssetMenu(menuName = "Project Zombie Survival/Entities/New Entity Database")]
    public class EntityDatabase : ScriptableObject {
        [Header("Entities")]
        [SerializeField] private List<EntityData> entities;

        public List<EntityData> Entities => entities;

        [SerializeField] private Dictionary<string, EntityData> entitiesById;

        public void Initialize() {
            entitiesById = new Dictionary<string, EntityData>();

            for (int i = 0; i < entities.Count; i++) {
                if (!entitiesById.ContainsKey(entities[i].ID)) {
                    entitiesById.Add(entities[i].ID, entities[i]);
                }
                else {
                    Debug.LogError($"[Entity Database] - ERROR: Database already contains entity ID '{entities[i].ID}'.");
                }
            }
        }

        public EntityData GetEntityData(string aEntityId) {
            EntityData lEntity = null;

            if (entitiesById.ContainsKey(aEntityId) == true) {
                lEntity = entitiesById[aEntityId];
            }
            else {
                Debug.LogError($"[Entity Database] - ERROR: Entity ID '{aEntityId}' not found.");
            }

            return lEntity;
        }

    }
}
