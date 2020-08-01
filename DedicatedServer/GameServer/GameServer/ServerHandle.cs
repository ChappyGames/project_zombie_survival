using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer {
    class ServerHandle {

        public static void WelcomeReceived(int aFromClient, Packet aPacket) {

            int lClientIdCheck = aPacket.ReadInt();
            string lUsername = aPacket.ReadString();

            Console.WriteLine($"{Server.clients[aFromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {aFromClient}.");
            if (aFromClient != lClientIdCheck) {
                Console.WriteLine($"Player \"{lUsername}\" (ID: {aFromClient}) has assumed the wrong client ID ({lClientIdCheck})!");
            } else {
                Console.WriteLine($"Hello {lUsername}!");
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
    }
}
