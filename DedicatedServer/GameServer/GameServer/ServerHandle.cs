using System;
using System.Collections.Generic;
using System.Text;

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

            //TODO: Send player into the game.
        }

        public static void UDPTestReceived(int aFromClient, Packet aPacket) {
            string lMessage = aPacket.ReadString();

            Console.WriteLine($"Received packet via UDP. Contains message: {lMessage}");
        }
    }
}
