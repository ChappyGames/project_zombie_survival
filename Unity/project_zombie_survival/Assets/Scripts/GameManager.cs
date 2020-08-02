using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public void SpawnPlayer(int aId, string aUsername, Vector3 aPosition, Quaternion aRotation, string aWeaponId) {
        GameObject lPlayer;

        if (aId == Client.instance.id) {
            lPlayer = Instantiate(localPlayerPrefab, aPosition, aRotation);
        } else {
            lPlayer = Instantiate(playerPrefab, aPosition, aRotation);
        }

        lPlayer.GetComponent<PlayerManager>().Initialize(aId, aUsername, aWeaponId);
        players.Add(aId, lPlayer.GetComponent<PlayerManager>());
    }
}
