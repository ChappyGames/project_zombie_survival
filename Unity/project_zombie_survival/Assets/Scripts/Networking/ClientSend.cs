using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour {

    private static void SendTCPData(Packet aPacket) {
        aPacket.WriteLength();
        Client.instance.tcp.SendData(aPacket);
    }

    private static void SendUDPData(Packet aPacket) {
        aPacket.WriteLength();
        Client.instance.udp.SendData(aPacket);
    }

    #region Packets
    public static void WelcomeReceived() {
        using (Packet lPacket = new Packet((int)ClientPackets.WELCOME_RECEIVED)) {
            lPacket.Write(Client.instance.id);
            lPacket.Write(UIManager.instance.usernameField.text);

            SendTCPData(lPacket);
        }
    }

    public static void UDPTestReceived() {
        using (Packet lPacket = new Packet((int)ClientPackets.UDP_TEST_RECEIVED)) {
            lPacket.Write("Received a UDP packet.");

            SendUDPData(lPacket);
        }
    }
    #endregion
}
