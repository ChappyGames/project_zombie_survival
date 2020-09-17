using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using ChappyGames.Server.Networking;
using ChappyGames.Server.InventorySystem;

namespace ChappyGames.Server.Entities {
    public class Mob : Entity {

        public class OnEntityDamagedEvent : UnityEvent<float> { }

        [Header("Mob Properties")]
        [SerializeField] protected float health;
        [SerializeField] protected float maxHealth;

        [SerializeField] protected float rawMoveSpeed = 5f;

        protected Inventory inventory;

        public float Health { get { return health; } }
        public float MoveSpeed { get { return rawMoveSpeed / Constants.TICKS_PER_SECOND; } }
        public Inventory Inventory { get { return inventory; } }
        public OnEntityDamagedEvent OnEntityDamaged { get; private set; } = new OnEntityDamagedEvent();
        public UnityEvent OnEntityDeath { get; private set; } = new UnityEvent();

        public override void Initialize(int aId, EntityType aType) {
            base.Initialize(aId, aType);

            health = maxHealth;
            inventory = new Inventory(this);

            OnEntityDamaged.AddListener(EntityDamaged);
            OnEntityDeath.AddListener(EntityDeath);
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();

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
                
                OnEntityDeath.Invoke();
                transform.position = new Vector3(0f, 0f, 0f);
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
    }
}
