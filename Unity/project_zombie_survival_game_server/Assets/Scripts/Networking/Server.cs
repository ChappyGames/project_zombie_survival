using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server {
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

    public delegate void PacketHandler(int aFromClient, Packet aPacket);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int aMaxPlayer, int aPort) {
        MaxPlayers = aMaxPlayer;
        Port = aPort;

        Debug.Log("Starting server...");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log("Server started on port: " + Port.ToString());
    }

    private static void TCPConnectCallback(IAsyncResult aResult) {
        TcpClient lClient = tcpListener.EndAcceptTcpClient(aResult);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        Debug.Log("Incoming connection from " + lClient.Client.RemoteEndPoint + "...");

        for (int i = 1; i <= MaxPlayers; i++) {
            if (clients[i].tcp.socket == null) {
                clients[i].tcp.Connect(lClient);
                return;
            }
        }

        Debug.Log(lClient.Client.RemoteEndPoint + " failed to connect: Server full!");
    }

    private static void UDPReceiveCallback(IAsyncResult aResult) {
        try {
            IPEndPoint lClientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] lData = udpListener.EndReceive(aResult, ref lClientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (lData.Length < 4) {
                return;
            }

            using (Packet lPacket = new Packet(lData)) {
                int lClientId = lPacket.ReadInt();

                if (lClientId == 0) {
                    return;
                }

                if (clients[lClientId].udp.endPoint == null) {
                    clients[lClientId].udp.Connect(lClientEndPoint);
                    return;
                }

                if (clients[lClientId].udp.endPoint.ToString() == lClientEndPoint.ToString()) {
                    clients[lClientId].udp.HandleData(lPacket);
                }
            }
        }
        catch (Exception e) {
            // If you see the error 'Error receiving UDP data: System.ObjectDisposedException: Cannot access a disposed object.' when closing the server in the editor, know that this error is by design.
            Debug.Log($"Error receiving UDP data: {e}");
        }
    }

    public static void SendUDPData(IPEndPoint aClientEndPoint, Packet aPacket) {
        try {
            if (aClientEndPoint != null) {
                udpListener.BeginSend(aPacket.ToArray(), aPacket.Length(), aClientEndPoint, null, null);
            }
        }
        catch (Exception e) {
            Debug.Log($"Error sending data to {aClientEndPoint} via UDP: {e}");
        }
    }

    private static void InitializeServerData() {
        for (int i = 1; i <= MaxPlayers; i++) {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>() {
                { (int)ClientPackets.WELCOME_RECEIVED, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.PLAYER_MOVEMENT, ServerHandle.PlayerMovement },
                { (int)ClientPackets.PLAYER_ATTACK, ServerHandle.PlayerAttack }
            };
        Debug.Log("Initialized packets.");
    }

    public static void Stop() {
        tcpListener.Stop();
        udpListener.Close();
    }
}
