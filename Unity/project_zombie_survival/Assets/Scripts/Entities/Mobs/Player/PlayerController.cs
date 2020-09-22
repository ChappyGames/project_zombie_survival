using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.Networking;
using ChappyGames.Client.Entities;

public class PlayerController : MonoBehaviour {

    public Camera camera;
    public GameObject playerModel;
    public Player parent;
    public Vector3 cameraPosOffset;

    private Plane groundPlane;

    private void Awake() {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        parent = GetComponent<Player>();
    }

    private void FixedUpdate() {
        SendInputToServer();
    }

    private void Update() {
        RotateToCursor();

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            ClientSend.PlayerAttack(playerModel.transform.forward);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            ClientSend.PlayerAction();
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            PlayerUIManager.Instance.ToggleInventory();
        }
    }

    private void RotateToCursor() {
        Ray lCameraRay = camera.ScreenPointToRay(Input.mousePosition);
        float lRayLength;
        Vector3 lPointToLook = Vector3.zero;

        if (groundPlane.Raycast(lCameraRay, out lRayLength)) {
            lPointToLook = lCameraRay.GetPoint(lRayLength);
            Debug.DrawLine(lCameraRay.origin, lPointToLook, Color.blue);

            playerModel.transform.LookAt(new Vector3(lPointToLook.x, transform.position.y, lPointToLook.z));
        }

        Vector3 lPlayerToCursorDistance = lPointToLook - transform.position;

        camera.transform.position = cameraPosOffset + new Vector3(transform.position.x, 0f, transform.position.z);
        if (parent.CurrentWeapon != null) {
            camera.transform.position += lPlayerToCursorDistance * (parent.CurrentWeapon.Range / 100f);
        }
    }

    private void SendInputToServer() {
        bool[] lInputs = new bool[] {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D)
        };

        ClientSend.PlayerMovement(lInputs);
    }

}
