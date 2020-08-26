using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityAttack {
    bool TryPerformAttack(Vector3 aViewDirection);
}

public class EntityAttack : MonoBehaviour, IEntityAttack {

    protected Entity parent;

    protected bool readyToAttack = true;

    public virtual float Damage { get { return 1.0f; } }
    public virtual float AttackCooldown { get { return 1.0f; } }

    public virtual void Initialize(Entity aEntity) {
        parent = aEntity;
        Debug.Log($"[Entity Attack] - Entity of type '{parent.Type}' with ID '{parent.ID}' initialized attack.");
    }

    public virtual bool TryPerformAttack(Vector3 aViewDirection) {
        if (readyToAttack == false) {
            return false;
        }

        Attack(aViewDirection);
        return true;
    }

    protected virtual void Attack(Vector3 aViewDirection) {
        Debug.Log($"[Entity Attack] - Entity of type '{parent.Type}' with ID '{parent.ID}' has started attacking.");
        Debug.DrawRay(transform.position, aViewDirection, Color.green, 5f, false);
        if (Physics.Raycast(transform.position, aViewDirection, out RaycastHit aHit)) {
            Entity lEntityHit = aHit.collider.GetComponentInParent<Entity>();
            if (lEntityHit != null) {
                Debug.Log($"[Entity Attack] - Entity of type '{parent.Type}' with ID '{parent.ID}' has hit entity of type '{lEntityHit.Type}' with ID '{lEntityHit.ID}' for {Damage} damage.");
                lEntityHit.TakeDamage(Damage);
            }
        }

        
    }

    protected virtual IEnumerator WaitForAttackCooldown() {
        readyToAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        readyToAttack = true;
    }
}
