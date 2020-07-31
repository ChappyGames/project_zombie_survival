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

        public static void UDPTest(int aToClient) {
            using (Packet lPacket = new Packet((int)ServerPackets.UDP_TEST)) {
                lPacket.Write("A test packet for UDP.");

                SendUDPData(aToClient, lPacket);
            }
        }
        #endregion
    }
}
