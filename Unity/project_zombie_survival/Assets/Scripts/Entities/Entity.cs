using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType {
    ENTITY_NONE = 0,
    ENTITY_PLAYER = 1,
    ENTITY_ZOMBIE = 2
}

public interface IEntity {
    int ID { get; }
    EntityType Type { get; }
    Transform Transform { get; }

    void SetHealth(float aHealth);
    void OnRespawn();
}

public class Entity : MonoBehaviour, IEntity {

    protected float maxHealth;
    protected float health;

    protected int id;
    protected EntityType type;

    public int ID { get { return id; } }
    public EntityType Type { get { return type; } }
    public virtual Transform Transform { get { return transform; } }
    public float MaxHealth { get { return maxHealth; } }
    public float Health { get { return health; } }


    public virtual void Initialize(int aId, EntityType aType) {
        id = aId;
        type = aType;

        health = maxHealth;
    }

    public virtual void SetHealth(float aHealth) {
        health = aHealth;

        if (health <= 0f) {
            OnDie();
        }
    }

    protected virtual void OnDie() {}

    public virtual void OnRespawn() {}
}
