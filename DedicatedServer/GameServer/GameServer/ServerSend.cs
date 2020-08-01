using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer {
    class ServerSend {

        private static void SendTCPData(int aToClient, Packet aPacket) {
            aPacket.WriteLength();
            Server.clients[aToClient].tcp.SendData(aPacket);
        }

        private static void SendUDPData(int aToClient, Packet aPacket) {
            aPacket.WriteLength();
            Server.clients[aToClient].udp.SendData(aPacket);
        }

        private static void SendTCPDataToAll(Packet aPacket) {
            aPacket.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                Server.clients[i].tcp.SendData(aPacket);
            }
        }

        private static void SendTCPDataToAll(int aExceptClient, Packet aPacket) {
            aPacket.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                if (i != aExceptClient) {
                    Server.clients[i].tcp.SendData(aPacket);
                }
            }
        }

        private static void SendUDPDataToAll(Packet aPacket) {
            aPacket.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                Server.clients[i].udp.SendData(aPacket);
            }
        }

        private static void SendUDPDataToAll(int aExceptClient, Packet aPacket) {
            aPacket.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                if (i != aExceptClient) {
                    Server.clients[i].udp.SendData(aPacket);
                }
            }
        }

        #region Packets
        public static void Welcome(int aToClient, string aMessage) {
            using (Packet lPacket = new Packet((int)ServerPackets.WELCOME)) {
                lPacket.Write(aMessage);
                lPacket.Write(aToClient);

                SendTCPData(aToClient, lPacket);
            }
        }

        public static void SpawnPlayer(int aToClient, Player aPlayer) {
            using (Packet lPacket = new Packet((int)ServerPackets.SPAWN_PLAYER)) {
                lPacket.Write(aPlayer.id);
                lPacket.Write(aPlayer.username);
                lPacket.Write(aPlayer.pos);
                lPacket.Write(aPlayer.rotation);

                SendTCPData(aToClient, lPacket);
            }
        }

        public static void PlayerPosition(Player aPlayer) {
            using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_POS)) {
                lPacket.Write(aPlayer.id);
                lPacket.Write(aPlayer.pos);

                SendUDPDataToAll(lPacket);
            }
        }

        public static void PlayerRotation(Player aPlayer) {
            using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_ROTATION)) {
                lPacket.Write(aPlayer.id);
                lPacket.Write(aPlayer.rotation);

                SendUDPDataToAll(aPlayer.id, lPacket);
            }
        }
        #endregion
    }
}
