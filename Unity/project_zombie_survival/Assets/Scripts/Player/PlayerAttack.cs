using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] private PlayerController player;
    [SerializeField] private AudioSource weaponAudioSource;

    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject attackPoint;

    //Perhaps store this in a serialized class that is also used by the scriptable object weapon data.
    private int maxAmmo;
    private int currentAmmo;

    private float damage;

    private float reloadSpeed;
    private float firingSpeed;
    private float accuracy;
    private float range;
    private float criticalChance;

    private bool readyToFire = true;

    private float spreadFactor;

    private void Start() {
        CalculateAttackStats();
    }

    public bool TryPerformAttack() {
        if (readyToFire == false) {
            return false;
        }

        if (currentAmmo > 0) {
            PerformAttack();
            readyToFire = false;
            currentAmmo--;
            return true;
        } else {
            StartCoroutine(WaitForReload());
            return false;
        }
    }

    private void PerformAttack() {
        Vector2 lDir = player.MousePosToPlayer;

        if (Mathf.Abs(lDir.x) > spreadFactor || Mathf.Abs(lDir.y) > spreadFactor) {

            //Apply the appropriate spread factor.
            lDir.x += Random.Range(-spreadFactor, spreadFactor);
            lDir.y += Random.Range(-spreadFactor, spreadFactor);
        }

        Debug.DrawRay(transform.position, new Vector3(lDir.x, lDir.y, 0f), Color.green, 1f, false);
        Debug.Log("Firing");

        weaponAudioSource.clip = weapon.FireSound;
        weaponAudioSource.Play();

        RaycastHit2D lHit = Physics2D.Raycast(new Vector2(attackPoint.transform.position.x, attackPoint.transform.position.y), lDir, range);

        if (lHit.collider != null) {
            Debug.Log("We just shot something!");
        }

        StartCoroutine(WaitForFireSpeed());
    }

    private IEnumerator WaitForFireSpeed() {
        yield return new WaitForSeconds(firingSpeed);
        readyToFire = true;
    }

    private IEnumerator WaitForReload() {
        readyToFire = false;
        Debug.Log("reloading");
        weaponAudioSource.clip = weapon.ReloadSound;
        weaponAudioSource.Play();
        yield return new WaitForSeconds(reloadSpeed);
        currentAmmo = maxAmmo;
        Debug.Log("reload complete");
        readyToFire = true;
    }

    private void CalculateAttackStats() {

        //TODO: ADD PLAYER ATTRIBUTE MODIFIERS!!
        maxAmmo = weapon.AmmoCapacity;
        damage = weapon.Damage;
        reloadSpeed = weapon.ReloadSpeed;
        firingSpeed = weapon.FiringSpeed;
        accuracy = weapon.Accuracy;
        range = weapon.Range;
        criticalChance = weapon.CriticalChance;

        //TODO: DEBUG TEST
        currentAmmo = maxAmmo;
        spreadFactor = (100f - accuracy) / 100f;
        Debug.Log(spreadFactor);
    }
}
