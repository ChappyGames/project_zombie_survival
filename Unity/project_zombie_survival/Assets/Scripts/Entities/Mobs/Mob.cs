using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.InventorySystem;

namespace ChappyGames.Client.Entities {

    public interface IMob : IEntity {

        Inventory Inventory { get; }

        void SetHealth(float aHealth);
        void OnDie();
        void OnRespawn();
    }

    public class Mob : Entity {

        [Header("Mob Properties")]
        [SerializeField] protected float health;
        [SerializeField] protected float maxHealth;

        protected Inventory inventory;

        public float MaxHealth { get { return maxHealth; } }
        public float Health { get { return health; } }
        public Inventory Inventory { get { return inventory; } }

        public override void Initialize(int aId, EntityType aType, Packet aPacket) {
            base.Initialize(aId, aType, aPacket);

            health = maxHealth;
            inventory = new Inventory(this);
        }

        public virtual void SetHealth(float aHealth) {
            health = aHealth;

            if (health <= 0f) {
                OnDie();
            }
        }

        protected virtual void OnDie() { }

        public virtual void OnRespawn() { }
    }
}
