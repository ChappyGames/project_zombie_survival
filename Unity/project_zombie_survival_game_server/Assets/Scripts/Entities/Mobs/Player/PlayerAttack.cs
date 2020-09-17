using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Server.Networking;
using ChappyGames.Server.Items;
using ChappyGames.Server.InventorySystem;

namespace ChappyGames.Server.Entities {

    public class PlayerAttack : EntityAttack {

        [SerializeField] protected Transform attackOrigin;

        private int currentAmmo;

        private Player Parent => (Player)parent;

        public Weapon CurrentWeapon { get { return Parent.Inventory.PrimaryWeapon; } }

        public override float Damage { get { return CurrentWeapon.Damage; } }

        public override float AttackCooldown { get { return CurrentWeapon.FiringSpeed; } }

        public void Initialize(Player aPlayer) {
            base.Initialize(aPlayer);
            /* TEST FUNCTIONALITY */
            InventoryItem lTestItem = new InventoryItem(ItemType.ITEM_WEAPON, "pistol_beta_tomcat");
            Parent.Inventory.AddItem(lTestItem);
            Parent.Inventory.UseItem(lTestItem);
            currentAmmo = CurrentWeapon.AmmoCapacity;
        }

        public override bool TryPerformAttack(Vector3 aViewDirection) {
            
            if (readyToAttack == false || CurrentWeapon == false) {
                return false;
            }

            if (currentAmmo > 0) {
                Attack(aViewDirection);
                return true;
            }
            else {
                StartCoroutine(ReloadCooldown());
                return false;
            }
        }

        protected override void Attack(Vector3 aViewDirection) {

            ServerSend.WeaponFire(parent);

            // Apply the appropriate spread factor for this weapon.
            aViewDirection.x += Random.Range(-(100f - CurrentWeapon.Accuracy) / 100f, (100f - CurrentWeapon.Accuracy) / 100f);
            aViewDirection.z += Random.Range(-(100f - CurrentWeapon.Accuracy) / 100f, (100f - CurrentWeapon.Accuracy) / 100f);

            Debug.Log($"[Player Attack] - Player '{Parent.username}' has started attacking.");
            Debug.DrawRay(attackOrigin.position, aViewDirection, Color.green, 5f, false);
            if (Physics.Raycast(attackOrigin.position, aViewDirection, out RaycastHit aHit)) {
                Mob lEntityHit = aHit.collider.GetComponentInParent<Mob>();
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
            Debug.Log($"[Player Attack] - Player '{Parent.username}' has started reloading.");
            ServerSend.WeaponReload(Parent);
            yield return new WaitForSeconds(CurrentWeapon.ReloadSpeed);
            currentAmmo = CurrentWeapon.AmmoCapacity;
            readyToAttack = true;
            Debug.Log($"[Player Attack] - Player '{Parent.username}' has finished reloading. They can now attack again.");
        }
    }
}
