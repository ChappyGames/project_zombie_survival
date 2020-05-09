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

        Ray lCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float lRayLength;
        Vector3 lPointToLook = Vector3.zero;

        if (groundPlane.Raycast(lCameraRay, out lRayLength)) {

            lPointToLook = lCameraRay.GetPoint(lRayLength);
            Debug.DrawLine(lCameraRay.origin, lPointToLook, Color.blue);
        }

        Vector3 lDir = lPointToLook;

        if (Mathf.Abs(lDir.x) > spreadFactor || Mathf.Abs(lDir.z) > spreadFactor) {

            //Apply the appropriate spread factor.
            lDir.x += Random.Range(-spreadFactor, spreadFactor);
            lDir.z += Random.Range(-spreadFactor, spreadFactor);
        }

        Debug.DrawRay(transform.position, new Vector3(lDir.x, 0f, lDir.z), Color.green, 1f, false);
        Debug.Log("Firing");

        weaponAudioSource.clip = weapon.FireSound;
        weaponAudioSource.Play();

        RaycastHit lHit;
        Physics.Raycast(new Vector3(attackPoint.transform.position.x, attackPoint.transform.position.y, attackPoint.transform.position.z), lDir, out lHit);

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
