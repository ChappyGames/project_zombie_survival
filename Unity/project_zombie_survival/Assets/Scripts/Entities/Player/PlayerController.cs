using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerController : NetworkBehaviour {

    [SerializeField] private NetworkIdentity identity;

    [SerializeField] private Player playerPrefab;
    [SerializeField] private NetworkTransformChild playerTransform;
    
    [SerializeField] private Player player;

    public override void OnStartClient() {
        base.OnStartClient();

        CmdSpawnPlayer();
    }

    private void Update() {
        if (hasAuthority == true) {
            player?.Process();
        }
    }

    private void FixedUpdate() {
        if (hasAuthority == true) {
            player?.FixedProcess();
        }
    }

    private void OnPlayerDeath() {

        Destroy(player);

        if (hasAuthority == true) {
            GameController.instance.respawnCanvas.gameObject.SetActive(true);
        }
    }

    [Command]
    public void CmdSpawnPlayer() {

        player = GameObject.Instantiate(playerPrefab);
        NetworkServer.Spawn(player.gameObject, base.connectionToClient);
        //CmdRequestAuthority(player.GetComponent<NetworkIdentity>());
        //playerTransform.target = player.transform;
        player.transform.position = new Vector3(0, 1, 0);

        RpcSpawnPlayer(player.gameObject);

    }

    [ClientRpc]
    public void RpcSpawnPlayer(GameObject aObj) {
        player = aObj.GetComponent<Player>();
        player.Initialize(OnPlayerDeath);
    }
    public void CmdRequestAuthority(NetworkIdentity aOtherId) {
        Debug.Log("Received request authority for " + gameObject.name);
        aOtherId.AssignClientAuthority(connectionToClient);
    }
}
