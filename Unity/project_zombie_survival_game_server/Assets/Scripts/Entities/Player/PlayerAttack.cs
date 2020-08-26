using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : EntityAttack {

    [SerializeField] private string currentWeaponId;

    [SerializeField] protected Transform attackOrigin;

    private int currentAmmo;
    private float spreadFactor;

    private Player Parent => (Player)parent;

    public Weapon CurrentWeapon { get { return ItemManager.Instance.GetWeapon(currentWeaponId); } }

    public override float Damage { get { return CurrentWeapon.Damage; } }

    public override float AttackCooldown { get { return CurrentWeapon.FiringSpeed; } }

    public void Initialize(Player aPlayer) {
        base.Initialize(aPlayer);
        currentAmmo = CurrentWeapon.AmmoCapacity;
        spreadFactor = (100f - CurrentWeapon.Accuracy) / 100f;
        Debug.Log($"[Player Attack] - Player '{aPlayer.username}' equipped weapon '{currentWeaponId}'.");
    }

    public override bool TryPerformAttack(Vector3 aViewDirection) {
        if (readyToAttack == false) {
            return false;
        }

        if (currentAmmo > 0) {
            Attack(aViewDirection);
            return true;
        } else {
            StartCoroutine(ReloadCooldown());
            return false;
        }
    }

    protected override void Attack(Vector3 aViewDirection) {

        ServerSend.WeaponFire(parent);

        // Apply the appropriate spread factor for this weapon.
        aViewDirection.x += Random.Range(-spreadFactor, spreadFactor);
        aViewDirection.z += Random.Range(-spreadFactor, spreadFactor);

        Debug.Log($"[Player Attack] - Player '{Parent.username}' has started attacking.");
        Debug.DrawRay(attackOrigin.position, aViewDirection, Color.green, 5f, false);
        if (Physics.Raycast(attackOrigin.position, aViewDirection, out RaycastHit aHit)) {
            Entity lEntityHit = aHit.collider.GetComponentInParent<Entity>();
            if (lEntityHit != null) {
                Debug.Log($"[Player Attack] - Player '{Parent.username}' has hit entity '{lEntityHit.Type}' with ID '{lEntityHit.ID}' for {CurrentWeapon.Damage} damage.");
                lEntityHit.TakeDamage(CurrentWeapon.Damage);
            }
        }

        currentAmmo--;
        StartCoroutine(WaitForAttackCooldown());
    }

    private IEnumerator ReloadCooldown() {
        readyToAttack = false;
        //Debug.Log($"[Player Attack] - Player '{parent.username}' has started reloading.");
        ServerSend.WeaponReload(Parent);
        yield return new WaitForSeconds(CurrentWeapon.ReloadSpeed);
        currentAmmo = CurrentWeapon.AmmoCapacity;
        readyToAttack = true;
        //Debug.Log($"[Player Attack] - Player '{parent.username}' has finished reloading. They can now attack again.");
    }
}
