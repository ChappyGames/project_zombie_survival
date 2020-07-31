using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer {
    class ServerSend {

        private static void SendTCPData(int aToClient, Packet aPacket) {
            aPacket.WriteLength();
            Server.clients[aToClient].tcp.SendData(aPacket);
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

        public static void Welcome(int aToClient, string aMessage) {
            using (Packet lPacket = new Packet((int)ServerPackets.WELCOME)) {
                lPacket.Write(aMessage);
                lPacket.Write(aToClient);

                SendTCPData(aToClient, lPacket);
            }
        }
    }
}
