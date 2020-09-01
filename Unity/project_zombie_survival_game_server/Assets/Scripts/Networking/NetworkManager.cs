using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChappyGames.Entities;

namespace ChappyGames.Networking {

    public class NetworkManager : Singleton<NetworkManager> {

        public GameObject playerPrefab;

        private void Start() {

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;

            Server.Start(50, 42069);
        }

        private void OnApplicationQuit() {
            Server.Stop();
        }

        public Player InstantiatePlayer() {
            return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        }
    }
}
