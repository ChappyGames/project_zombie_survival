using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Client.Entities {

    [CreateAssetMenu(menuName = "Project Zombie Survival/Entities/New Entity")]
    public class EntityData : ScriptableObject {

        [SerializeField] private string id;
        [SerializeField] private string entityName;
        [SerializeField] private GameObject entityObject;

        public string ID { get { return id; } }
        public string EntityName { get { return entityName; } }
        public GameObject EntityObject { get { return entityObject; } }
    }
}
