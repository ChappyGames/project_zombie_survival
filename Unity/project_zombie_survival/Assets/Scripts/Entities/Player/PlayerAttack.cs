using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] private PlayerController player;
    [SerializeField] private AudioSource weaponAudioSource;

    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject attackPoint;

    private int currentAmmo;

    public int MaxAmmo => weapon.AmmoCapacity;
    public float Damage => weapon.Damage;
    public float ReloadSpeed => weapon.ReloadSpeed;
    public float FiringSpeed => weapon.FiringSpeed;
    public float Accuracy => weapon.Accuracy;
    public float Range => weapon.Range;
    public float CriticalChance => weapon.CriticalChance;

    private bool readyToFire = true;

    private float spreadFactor;

    private Plane groundPlane;

    private void Start() {

        CalculateAttackStats();

        groundPlane = new Plane(Vector3.up, Vector3.zero);
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

        Vector3 lDir = attackPoint.transform.forward;

        if (Mathf.Abs(lDir.x) > spreadFactor || Mathf.Abs(lDir.z) > spreadFactor) {

            //Apply the appropriate spread factor.
            lDir.x += Random.Range(-spreadFactor, spreadFactor);
            lDir.z += Random.Range(-spreadFactor, spreadFactor);
        }

        Debug.DrawRay(attackPoint.transform.position, lDir, Color.green, 1f, false);
        Debug.Log("Firing");

        weaponAudioSource.clip = weapon.FireSound;
        weaponAudioSource.Play();

        RaycastHit lHit;
        Physics.Raycast(attackPoint.transform.position, lDir, out lHit);

        if (lHit.collider != null) {

            Debug.Log("We just shot something!");
        }

        StartCoroutine(WaitForFireSpeed());
    }

    private IEnumerator WaitForFireSpeed() {

        yield return new WaitForSeconds(FiringSpeed);
        readyToFire = true;
    }

    private IEnumerator WaitForReload() {

        readyToFire = false;
        Debug.Log("reloading");
        weaponAudioSource.clip = weapon.ReloadSound;
        weaponAudioSource.Play();
        yield return new WaitForSeconds(ReloadSpeed);
        currentAmmo = MaxAmmo;
        Debug.Log("reload complete");
        readyToFire = true;
    }

    private void CalculateAttackStats() {

        //TODO: DEBUG TEST
        currentAmmo = MaxAmmo;
        spreadFactor = (100f - Accuracy) / 100f;
        Debug.Log(spreadFactor);
    }
}
