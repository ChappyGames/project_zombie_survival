using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Client.Entities {

    public enum EntityType {
        ENTITY_NONE = 0,
        ENTITY_PLAYER = 1,
        ENTITY_ZOMBIE = 2,
        ENTITY_ITEM = 3
    }

    public interface IEntity {
        Guid ID { get; }
        EntityType Type { get; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
    }

    public class Entity : MonoBehaviour, IEntity {

        protected Guid id;
        protected EntityType type;

        public Guid ID { get { return id; } }
        public EntityType Type { get { return type; } }
        public virtual Vector3 Position { get { return transform.position; } set { transform.position = value; } }
        public virtual Quaternion Rotation { get { return transform.rotation; } set { transform.rotation = value; } }

        public virtual void Initialize(Guid aId, EntityType aType, Packet aPacket) {
            id = aId;
            type = aType;

            EntityManager.Instance.RegisterEntity(this);
        }

        protected virtual void OnDestroy() {
            EntityManager.Instance.UnregisterEntity(this);
        }
    }
}
