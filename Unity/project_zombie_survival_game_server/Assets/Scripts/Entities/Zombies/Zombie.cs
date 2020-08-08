using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : Entity {

    private FieldOfView fov;
    private NavMeshAgent nav;

    private Player target;

    private void Start() {
        // IDs should start at 1
        Initialize(EntityManager.Instance.GetEntityCount((int)EntityType.ENTITY_ZOMBIE) + 1);
    }

    public void Initialize(int aId) {
        base.Initialize(aId, EntityType.ENTITY_ZOMBIE);

        fov = GetComponent<FieldOfView>();
        nav = GetComponent<NavMeshAgent>();

        nav.speed = rawMoveSpeed;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

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
        } else {

            // Navigate to target player. 
            nav.destination = target.transform.position;
        }

        ServerSend.EntityPosition(this);
        ServerSend.EntityRotation(this);
    }

    private void OnTargetDeath() {
        target.OnEntityDeath.RemoveListener(OnTargetDeath);
        target = null;
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(5f);

        health = maxHealth;
    }

}
