using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using ChappyGames.Server.Networking;

namespace ChappyGames.Server.Entities {

    [RequireComponent(typeof(FieldOfView))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : Mob {

        [Header("Zombie Properties")]
        public ZombieAttack attack;

        private FieldOfView fov;
        private NavMeshAgent nav;

        private Player target;

        private void Start() {
            // IDs should start at 1
            Initialize(EntityManager.Instance.GetEntityCount((int)EntityType.ENTITY_ZOMBIE) + 1);
        }

        public void Initialize(int aId) {
            
            fov = GetComponent<FieldOfView>();
            nav = GetComponent<NavMeshAgent>();

            nav.speed = rawMoveSpeed;

            OnEntityDeath.AddListener(OnZombieDeath);

            base.Initialize("zombie", aId, EntityType.ENTITY_ZOMBIE);

            attack.Initialize(this);
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();

            if (health <= 0f) {
                return;
            }

            if (target == null) {

                // Fetch a visible player if any.
                for (int i = 0; i < fov.visibleTargets.Count; i++) {
                    Player lPlayer = fov.visibleTargets[i].GetComponentInParent<Player>();
                    if (lPlayer != null) {
                        target = lPlayer;
                        target.OnEntityDeath.AddListener(OnTargetDeath);
                        break;
                    }
                }
            }
            else {

                // Navigate to target player. 
                nav.destination = target.transform.position;
            }

            ServerSend.EntityPosition(this);
            ServerSend.EntityRotation(this);
        }

        private void OnTargetDeath() {
            //Double check this setup at some point.
            target?.OnEntityDeath.RemoveListener(OnTargetDeath);
            target = null;
        }

        private void OnZombieDeath() {
            target = null;
            nav.SetDestination(transform.position);

            /* TEMP */
            Item lItemInstance = Instantiate(EntityManager.Instance.ItemPrefab, transform.position, transform.rotation);
            lItemInstance.Initialize(EntityManager.Instance.GetEntityCount((int)EntityType.ENTITY_ITEM) + 1, transform.position, "pistol_beta_tomcat");
        }
    }
}
