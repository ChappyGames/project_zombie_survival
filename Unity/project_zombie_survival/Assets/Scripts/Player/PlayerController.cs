using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour {

    [SerializeField] private PlayerAttack attack;
    //[SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float speed = 1.0f;
    private Vector3 input;

    private Vector2 mousePos;
    private Vector2 mousePosToPlayer;

    public Vector2 MousePos { get { return mousePos; } }
    public Vector2 MousePosToPlayer { get { return mousePosToPlayer; } }

    private void Awake() {
        
    }

    private void Start() {
        
    }

    private void Update() {
        if (isLocalPlayer == true) {
            if (Input.GetMouseButton(0) == true) {
                attack.TryPerformAttack();
            }
        }
    }

    private void FixedUpdate() {
        if (isLocalPlayer == true) {
            // Movement
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            input.z = 0f;

            rb.AddForce(input * speed, ForceMode.Impulse);

            // Rotation
            // Grabs the current mouse position on the screen
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));
            mousePosToPlayer = mousePos - new Vector2(transform.position.x, transform.position.y);
            // Rotates toward the mouse
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x)) * Mathf.Rad2Deg + 270f);

            //Set Camera in between player and cursor
            Vector2 lMidPoint = (mousePosToPlayer / 10.0f) + new Vector2(transform.position.x, transform.position.y);
            //Camera.main.transform.position = new Vector3(lMidPoint.x, lMidPoint.y, Camera.main.transform.position.z);
        }
    }
}
