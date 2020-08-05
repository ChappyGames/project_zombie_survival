using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Player : Entity {

    [SerializeField] private PlayerAttack attack;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Vector3 cameraPosOffset;
    
    private Vector3 input;
    private Vector3 inputVelocity;

    private Plane groundPlane;

    private UnityAction onDeath;

    public Rigidbody RB => rb;

    #region Life Cycle

    public new void Initialize(UnityAction aOnDeath) {

        onDeath = aOnDeath;
        groundPlane = new Plane(Vector3.up, Vector3.zero);

        //base.Initialize();
    }

    public void Process() {

        //Debug.Log(hasAuthority);

        if (Input.GetMouseButton(0) == true) {
                
            attack.TryPerformAttack();
        }

        // Get movement values
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        inputVelocity = input * speed;

        // Rotate to cursor
        Ray lCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float lRayLength;
        Vector3 lPointToLook = Vector3.zero;

        if (groundPlane.Raycast(lCameraRay, out lRayLength)) {

            lPointToLook = lCameraRay.GetPoint(lRayLength);
            Debug.DrawLine(lCameraRay.origin, lPointToLook, Color.blue);

            transform.LookAt(new Vector3(lPointToLook.x, transform.position.y, lPointToLook.z));

            /* TODO: If the cursor is too close to the player, the rotation will change wildly. Perhaps limit the amount of rotation that can be done in a single frame. */
        }

        // Position camera between player and cursor
        Vector3 lPlayerToCursorDistance = lPointToLook - transform.position;

        Camera.main.transform.position = cameraPosOffset + new Vector3(transform.position.x, 0f, transform.position.z) + (lPlayerToCursorDistance * (attack.Range / 100f));
    }

    public void FixedProcess() {

        // Apply movement
        rb.velocity = inputVelocity;
    }

    #endregion

    /*
    public override void ModifyHealth(int aAmount) {


        base.ModifyHealth(aAmount);
    }

    protected override void OnDeath() {

        onDeath?.Invoke();
        Debug.Log("Player has died!");

        base.OnDeath();
    }
    */
    
}
