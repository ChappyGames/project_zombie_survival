using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Server.Entities {
    public class EntitySpawner : MonoBehaviour {

        [SerializeField] private string[] spawnableEntities;
        [SerializeField] private int maxEntities;

        [SerializeField] private float cooldownTimeMin;
        [SerializeField] private float cooldownTimeMax;

        private List<Entity> entities = new List<Entity>();
        private float cooldownTime = 0f;

        private void Start() {
            cooldownTime = Random.Range(cooldownTimeMin, cooldownTimeMax);
        }

        private void FixedUpdate() {
            
            if (cooldownTime <= 0f) {
                
                // Spawn an entity if we are not at cap entities
                if (entities.Count >= maxEntities) {
                    return;
                }

                int lRandomEntity = Random.Range(0, spawnableEntities.Length);

                EntityData lData = EntityManager.Instance.EntityDatabase.GetEntityData(spawnableEntities[lRandomEntity]);
                IEntity lEntity = Instantiate(lData.EntityObject, transform.position, transform.rotation).GetComponent<IEntity>();
                lEntity.Initialize(lData.ID, lData.EntityType);

                cooldownTime = Random.Range(cooldownTimeMin, cooldownTimeMax);
            } else {
                cooldownTime -= Time.fixedDeltaTime;
            }

        }

    }
}
