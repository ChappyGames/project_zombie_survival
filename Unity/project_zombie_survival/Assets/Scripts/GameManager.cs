using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public void SpawnPlayer(int aId, string aUsername, Vector3 aPosition, Quaternion aRotation) {
        GameObject lPlayer;

        if (aId == Client.instance.id) {
            lPlayer = Instantiate(localPlayerPrefab, aPosition, aRotation);
        } else {
            lPlayer = Instantiate(playerPrefab, aPosition, aRotation);
        }

        lPlayer.GetComponent<PlayerManager>().id = aId;
        lPlayer.GetComponent<PlayerManager>().username = aUsername;
        players.Add(aId, lPlayer.GetComponent<PlayerManager>());
    }
}
