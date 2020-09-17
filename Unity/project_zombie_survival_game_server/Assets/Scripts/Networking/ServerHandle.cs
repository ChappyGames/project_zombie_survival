using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChappyGames.Server.Networking {

    public class ServerHandle {
        public static void WelcomeReceived(int aFromClient, Packet aPacket) {

            int lClientIdCheck = aPacket.ReadInt();
            string lUsername = aPacket.ReadString();

            Debug.Log($"{Server.clients[aFromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {aFromClient}.");
            if (aFromClient != lClientIdCheck) {
                Debug.Log($"Player \"{lUsername}\" (ID: {aFromClient}) has assumed the wrong client ID ({lClientIdCheck})!");
            }
            else {
                Debug.Log($"Hello {lUsername}!");
            }

            Server.clients[aFromClient].SendIntoGame(lUsername);
        }

        public static void PlayerMovement(int aFromClient, Packet aPacket) {

            bool[] lInputs = new bool[aPacket.ReadInt()];
            for (int i = 0; i < lInputs.Length; i++) {
                lInputs[i] = aPacket.ReadBool();
            }
            Quaternion lRotation = aPacket.ReadQuaternion();

            Server.clients[aFromClient].player.SetInput(lInputs, lRotation);
        }

        public static void PlayerAttack(int aFromClient, Packet aPacket) {
            Vector3 lAttackDirection = aPacket.ReadVector3();

            Server.clients[aFromClient].player.attack.TryPerformAttack(lAttackDirection);
        }
    }
}
