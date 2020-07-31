using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour { 

    public static void Welcome(Packet aPacket) {
        string lMessage = aPacket.ReadString();
        int lMyId = aPacket.ReadInt();

        Debug.Log($"Message from server: {lMessage}");
        Client.instance.id = lMyId;
        //TODO: send welcome received packet
    }

}
