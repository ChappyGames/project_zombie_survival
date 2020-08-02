using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int id;
    public string username;
    
    public float health;
    public float maxHealth;

    public PlayerAttack attack;

    private float moveSpeed = 5f / Constants.TICKS_PER_SECOND;
    private bool[] inputs;

    public void Initialize(int aId, string aUsername) {
        id = aId;
        username = aUsername;
        health = maxHealth;
        inputs = new bool[4];

        attack.Initialize(this);
    }

    public void FixedUpdate() {

        if (health <= 0f) {
            return;
        }

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

    public void TakeDamage(float aDamage) {
        if (health <= 0f) {
            return;
        }

        health -= aDamage;
        if (health <= 0f) {
            health = 0f;
            transform.position = new Vector3(0f, 0f, 0f);
            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(this);
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(5f);

        health = maxHealth;
        ServerSend.PlayerRespawned(this);
        
    }
}
