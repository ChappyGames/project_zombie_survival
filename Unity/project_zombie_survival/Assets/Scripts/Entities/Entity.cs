using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Entity : NetworkBehaviour {

    [SerializeField] private int maxHealth;

    [SyncVar]
    private int health;

    protected virtual void Initialize() {
        health = maxHealth;
    }

    public virtual void ModifyHealth(int aAmount) {
        health = Mathf.Clamp(health + aAmount, 0, maxHealth);
        Debug.Log("Health: " + health);
        if (health <= 0) {
            OnDeath();
        }
    }

    [Command]
    public virtual void CmdModifyHealth(int aAmount) {
        Debug.Log("Server is modifying health");
        ModifyHealth(aAmount);
        //RpcModifyHealth(aAmount);
    }

    [ClientRpc]
    public virtual void RpcModifyHealth(int aAmount) {
        Debug.Log("Client is receiving modified health");
        ModifyHealth(aAmount);
    }

    protected virtual void OnDeath() {
        NetworkServer.Destroy(gameObject);
        Debug.Log("Entity has died!");
    }

    [Command]
    public virtual void CmdOnDeath() {
        OnDeath();
        RpcOnDeath();
    }

    [ClientRpc]
    public virtual void RpcOnDeath() {
        OnDeath();
    }
}
