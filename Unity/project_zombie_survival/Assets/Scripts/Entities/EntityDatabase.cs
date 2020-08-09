using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project Zombie Survival/Entities/New Entity Database")]
public class EntityDatabase : ScriptableObject {
    [Header("Players")]
    [SerializeField] private List<EntityData> playerEntities;

    public List<EntityData> PlayerEntities => playerEntities;

    [SerializeField] private Dictionary<string, EntityData> playersById;

    public void Initialize() {
        playersById = new Dictionary<string, EntityData>();

        for (int i = 0; i < playerEntities.Count; i++) {
            if (!playersById.ContainsKey(playerEntities[i].ID)) {
                playersById.Add(playerEntities[i].ID, playerEntities[i]);
            }
            else {
                Debug.LogError($"[Item Database] - ERROR: Database already contains weapon ID '{playerEntities[i].ID}'.");
            }
        }
    }

    public EntityData GetPlayerEntityData(string aEntityId) {
        EntityData lEntity = null;

        if (playersById.ContainsKey(aEntityId) == true) {
            lEntity = playersById[aEntityId];
        }
        else {
            Debug.LogError($"[Item Database] - ERROR: Weapon ID '{aEntityId}' not found.");
        }

        return lEntity;
    }
    
}
