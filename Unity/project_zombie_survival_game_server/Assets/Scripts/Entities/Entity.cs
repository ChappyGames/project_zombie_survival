using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EntityType {
    ENTITY_NONE = 0,
    ENTITY_PLAYER = 1,
    ENTITY_ZOMBIE = 2
}

public interface IEntity {
    int ID { get; }
    EntityType Type { get; }

    void ServerSpawnEntity(int aToClient);
}
public class Entity : MonoBehaviour, IEntity {

    public class OnEntityDamagedEvent : UnityEvent<float> { }

    [Header("Entity Properties")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;

    [SerializeField] protected float rawMoveSpeed = 5f;

    protected int id;
    protected EntityType type;

    public int ID { get { return id; } }
    public EntityType Type { get { return type; } }
    public float Health { get { return health; } }
    public float MoveSpeed { get { return rawMoveSpeed / Constants.TICKS_PER_SECOND; } }
    public OnEntityDamagedEvent OnEntityDamaged { get; private set; } = new OnEntityDamagedEvent();
    public UnityEvent OnEntityDeath { get; private set; } = new UnityEvent();

    public virtual void Initialize(int aId, EntityType aType) {
        id = aId;
        type = aType;

        health = maxHealth;

        OnEntityDamaged.AddListener(EntityDamaged);
        OnEntityDeath.AddListener(EntityDeath);

        EntityManager.Instance.RegisterEntity(this);
        SpawnToAllExistingPlayers();
    }

    protected virtual void SpawnToAllExistingPlayers() {
        foreach (Client lClient in Server.clients.Values) {
            if (lClient.player != null) {
                ServerSpawnEntity(lClient.id);
            }
        }
    }

    public virtual void ServerSpawnEntity(int aToClient) {
        ServerSend.SpawnEntity(aToClient, this);
    }


    protected virtual void FixedUpdate() {
        if (health <= 0f) {
            return;
        }
    }

    public virtual void TakeDamage(float aDamage) {
        if (health <= 0f) {
            return;
        }

        health -= aDamage;
        if (health <= 0f) {
            health = 0f;
            transform.position = new Vector3(0f, 0f, 0f);
            OnEntityDeath.Invoke();
        }

        OnEntityDamaged.Invoke(aDamage);
    }

    private void EntityDamaged(float aDamage) {
        ServerSend.EntityHealth(this);
    }

    private void EntityDeath() {
        ServerSend.EntityPosition(this);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(5f);

        health = maxHealth;
        ServerSend.EntityRespawned(this);
    }

    protected virtual void OnDestroy() {
        EntityManager.Instance.UnregisterEntity(this);
    }
}
