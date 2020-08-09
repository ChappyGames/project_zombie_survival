using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] private Transform attackOrigin;
    [SerializeField] private string currentWeaponId;

    private Player parent;

    private int currentAmmo;
    private float spreadFactor;
    private bool readyToFire = true;

    public Weapon CurrentWeapon { get { return ItemManager.Instance.GetWeapon(currentWeaponId); } }

    public void Initialize(Player aPlayer) {
        parent = aPlayer;
        currentAmmo = CurrentWeapon.AmmoCapacity;
        spreadFactor = (100f - CurrentWeapon.Accuracy) / 100f;
        Debug.Log($"[Player Attack] - Player '{parent.username}' equipped weapon '{currentWeaponId}'.");
    }

    public bool TryPerformAttack(Vector3 aViewDirection) {
        if (readyToFire == false) {
            return false;
        }

        if (currentAmmo > 0) {
            Attack(aViewDirection);
            readyToFire = false;
            currentAmmo--;
            return true;
        } else {
            StartCoroutine(WaitForReload());
            return false;
        }
    }

    private void Attack(Vector3 aViewDirection) {

        ServerSend.WeaponFire(parent);

        // Apply the appropriate spread factor for this weapon.
        aViewDirection.x += Random.Range(-spreadFactor, spreadFactor);
        aViewDirection.z += Random.Range(-spreadFactor, spreadFactor);

        Debug.Log($"[Player Attack] - Player '{parent.username}' has started attacking.");
        Debug.DrawRay(attackOrigin.position, aViewDirection, Color.green, 5f, false);
        if (Physics.Raycast(attackOrigin.position, aViewDirection, out RaycastHit aHit)) {
            Entity lEntityHit = aHit.collider.GetComponentInParent<Entity>();
            if (lEntityHit != null) {
                Debug.Log($"[Player Attack] - Player '{parent.username}' has hit entity '{lEntityHit.Type}' with ID '{lEntityHit.ID}' for {CurrentWeapon.Damage} damage.");
                lEntityHit.TakeDamage(CurrentWeapon.Damage);
            }
        }

        StartCoroutine(WaitForFireSpeed());
    }

    private IEnumerator WaitForFireSpeed() {
        yield return new WaitForSeconds(CurrentWeapon.FiringSpeed);
        readyToFire = true;
    }

    private IEnumerator WaitForReload() {
        readyToFire = false;
        Debug.Log($"[Player Attack] - Player '{parent.username}' has started reloading.");
        ServerSend.WeaponReload(parent);
        yield return new WaitForSeconds(CurrentWeapon.ReloadSpeed);
        currentAmmo = CurrentWeapon.AmmoCapacity;
        readyToFire = true;
        Debug.Log($"[Player Attack] - Player '{parent.username}' has finished reloading. They can now attack again.");
    }
}
