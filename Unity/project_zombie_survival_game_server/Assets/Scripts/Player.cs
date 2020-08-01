using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int id;
    public string username;

    private float moveSpeed = 5f / Constants.TICKS_PER_SECOND;
    private bool[] inputs;

    public void Initialize(int aId, string aUsername) {
        id = aId;
        username = aUsername;

        inputs = new bool[4];
    }

    public void FixedUpdate() {
        Vector2 lInputDirection = Vector2.zero;
        if (inputs[0]) {
            lInputDirection.y += 1;
        }

        if (inputs[1]) {
            lInputDirection.y -= 1;
        }
        if (inputs[2]) {
            lInputDirection.x -= 1;
        }
        if (inputs[3]) {
            lInputDirection.x += 1;
        }

        Move(lInputDirection);
    }

    private void Move(Vector2 aInputDirection) {

        Vector3 lMoveDirection = new Vector3(1,0,0) * aInputDirection.x + new Vector3(0,0,1) * aInputDirection.y;
        transform.position += lMoveDirection * moveSpeed;

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] aInput, Quaternion aRotation) {
        inputs = aInput;
        transform.rotation = aRotation;
    }
}
