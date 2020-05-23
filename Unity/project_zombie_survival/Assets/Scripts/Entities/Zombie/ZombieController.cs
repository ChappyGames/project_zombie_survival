using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(NavMeshAgent))]
public class ZombieController : MonoBehaviour {

    private FieldOfView fov;
    private NavMeshAgent nav;

    private PlayerController targetPlayer;

    private void Awake() {
        fov = GetComponent<FieldOfView>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Update() {

        if (targetPlayer == null) {
            
            // Fetch a visible player if any.
            for (int i = 0; i < fov.visibleTargets.Count; i++) {
                PlayerController lPlayer = fov.visibleTargets[i].GetComponent<PlayerController>();
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
}
