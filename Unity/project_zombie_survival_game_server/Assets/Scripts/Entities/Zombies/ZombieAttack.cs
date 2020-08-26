using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : EntityAttack {

    [SerializeField] GameObject attackCollider;

    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage;

    public override float Damage => damage;

    public override float AttackCooldown => attackCooldown;

    public override void Initialize(Entity aEntity) {
        base.Initialize(aEntity);

        StartCoroutine(FindTargets(Constants.SECONDS_PER_TICK));
    }

    private IEnumerator FindTargets(float aDelay) {
        while (true) {
            yield return new WaitForSeconds(aDelay);
            FindTargetsInRange();
        }
    }

    private void FindTargetsInRange() {
        Collider[] lTargets = Physics.OverlapBox(attackCollider.transform.position, new Vector3(0.1f, 0.1f, 1f), transform.rotation);

        for (int i = 0; i < lTargets.Length; i++) {
            Player lPlayer = lTargets[i].GetComponentInParent<Player>();

            if (lPlayer != null) {
                TryPerformAttack(transform.forward);
            }
        }
    }

    protected override void Attack(Vector3 aViewDirection) {

        // Send packet to clients about this zombie's attack

        Debug.Log($"[Zombie Attack] - Zombie with ID '{parent.ID}' has started attacking.");

        Collider[] lTargets = Physics.OverlapBox(attackCollider.transform.position, new Vector3(0.1f, 0.1f, 1f), transform.rotation);
        List<Entity> lEntitiesHit = new List<Entity>();

        for (int i = 0; i < lTargets.Length; i++) {
            Entity lEntity = lTargets[i].GetComponentInParent<Entity>();

            if (lEntity != null && lEntity != parent) {
                lEntitiesHit.Add(lEntity);
            }
        }

        for (int i = 0; i < lEntitiesHit.Count; i++) {
            Debug.Log($"[Zombie Attack] - Zombie with ID '{parent.ID}' has hit entity '{lEntitiesHit[i].Type}' with ID '{lEntitiesHit[i].ID}' for {Damage} damage.");
            lEntitiesHit[i].TakeDamage(Damage);
        }

        StartCoroutine(WaitForAttackCooldown());
    }
}
