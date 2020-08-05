using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : Entity {

    private FieldOfView fov;
    private NavMeshAgent nav;

    private Player targetPlayer;

    #region Life Cycle
    private void Awake() {
        fov = GetComponent<FieldOfView>();
        nav = GetComponent<NavMeshAgent>();

        //base.Initialize();
    }

    private void Update() {

        if (targetPlayer == null) {
            
            // Fetch a visible player if any.
            for (int i = 0; i < fov.visibleTargets.Count; i++) {
                Player lPlayer = fov.visibleTargets[i].GetComponent<Player>();
                if (lPlayer != null) {
                    targetPlayer = lPlayer;
                    break;
                }
            }
        } else {

            // Navigate to target player.
            nav.destination = targetPlayer.transform.position;
        }
    }

    #endregion

    /*
    public override void ModifyHealth(int aAmount) {

        //TODO: Blood splats? Hurt zombie sounds?

        base.ModifyHealth(aAmount);
    }

    [Command]
    public override void CmdModifyHealth(int aAmount) {

        base.CmdModifyHealth(aAmount);
    }

    protected override void OnDeath() {

        //gameObject.SetActive(false);
        Debug.Log("Zombie has died!");
        
        base.OnDeath();
    }

    [Command]
    public override void CmdOnDeath() {

        base.CmdOnDeath();
    }

    [ClientRpc]
    public override void RpcOnDeath() {

        base.RpcOnDeath();
    }
    */
}
