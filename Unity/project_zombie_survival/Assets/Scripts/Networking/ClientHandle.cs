using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour { 

    public static void Welcome(Packet aPacket) {
        string lMessage = aPacket.ReadString();
        int lMyId = aPacket.ReadInt();

        Debug.Log($"[Client Handle] - Message from server: {lMessage}");
        Client.instance.id = lMyId;
        //TODO: send welcome received packet
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void UDPTest(Packet aPacket) {
        string lMessage = aPacket.ReadString();

        Debug.Log($"[Client Handle] - Received packet via UDP. Contains message: {lMessage}");
        ClientSend.UDPTestReceived();
    }

}
