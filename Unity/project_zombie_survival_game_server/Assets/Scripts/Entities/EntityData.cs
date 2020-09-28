using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Server.Entities {

    [CreateAssetMenu(menuName = "Project Zombie Survival/Entities/New Entity")]
    public class EntityData : ScriptableObject {

        [SerializeField] private string id;
        [SerializeField] private string entityName;
        [SerializeField] private EntityType entityType;
        [SerializeField] private GameObject entityObject;

        public string ID => id;
        public string EntityName => entityName;
        public EntityType EntityType => entityType;
        public GameObject EntityObject => entityObject;
    }
}
