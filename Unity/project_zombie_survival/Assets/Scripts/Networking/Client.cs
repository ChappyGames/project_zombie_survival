using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour {

    public class TCP {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect() {
            socket = new TcpClient {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult aResult) {
            socket.EndConnect(aResult);

            if (!socket.Connected) {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet aPacket) {
            try {
                if (socket != null) {
                    stream.BeginWrite(aPacket.ToArray(), 0, aPacket.Length(), null, null);
                }
            } catch(Exception e) {
                Debug.Log($"[Client] - Error sending data to server via TCP: {e}");
            }
        }

        private void ReceiveCallback(IAsyncResult aResult) {
            try {
                int lByteLength = stream.EndRead(aResult);
                if (lByteLength <= 0) {
                    instance.Disconnect();
                    return;
                }

                byte[] lData = new byte[lByteLength];
                Array.Copy(receiveBuffer, lData, lByteLength);

                receivedData.Reset(HandleData(lData));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            } catch {
                Disconnect();
            }
        }

        private bool HandleData(byte[] aData) {
            int lPacketLength = 0;

            receivedData.SetBytes(aData);

            if (receivedData.UnreadLength() >= 4) {
                lPacketLength = receivedData.ReadInt();
                if (lPacketLength <= 0) {
                    return true;
                }
            }

            while (lPacketLength > 0 && lPacketLength <= receivedData.UnreadLength()) {
                byte[] lPacketBytes = receivedData.ReadBytes(lPacketLength);
                ThreadManager.ExecuteOnMainThread(() => {
                    using (Packet lPacket = new Packet(lPacketBytes)) {
                        int lPacketId = lPacket.ReadInt();
                        packetHandlers[lPacketId](lPacket);
                    }
                });

                lPacketLength = 0;

                if (receivedData.UnreadLength() >= 4) {
                    lPacketLength = receivedData.ReadInt();
                    if (lPacketLength <= 0) {
                        return true;
                    }
                }
            }

            if (lPacketLength <= 1) {
                return true;
            }

            return false;
        }

        private void Disconnect() {
            instance.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP {
        public UdpClient socket;
        public IPEndPoint endpoint;

        public UDP() {
            endpoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int aLocalPort) {
            socket = new UdpClient(aLocalPort);

            socket.Connect(endpoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet lPacket = new Packet()) {
                SendData(lPacket);
            }
        }

        private void ReceiveCallback(IAsyncResult aResult) {
            try {
                byte[] lData = socket.EndReceive(aResult, ref endpoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (lData.Length < 4) {
                    instance.Disconnect();
                    return;
                }

                HandleData(lData);
            } catch {
                Disconnect();
            }
        }

        public void SendData(Packet aPacket) {
            try {
                aPacket.InsertInt(instance.id);
                if (socket != null) {
                    socket.BeginSend(aPacket.ToArray(), aPacket.Length(), null, null);
                }
            } catch(Exception e) {
                Debug.Log($"[Client] - Error sending data to server via UDP: {e}");
            }
        }

        private void HandleData(byte[] aData) {
            using (Packet lPacket = new Packet(aData)) {
                int lPacketLength = lPacket.ReadInt();
                aData = lPacket.ReadBytes(lPacketLength);
            }

            ThreadManager.ExecuteOnMainThread(() => {
                using (Packet lPacket = new Packet(aData)) {
                    int lPacketId = lPacket.ReadInt();
                    packetHandlers[lPacketId](lPacket);
                }
            });
        }

        private void Disconnect() {
            instance.Disconnect();

            endpoint = null;
            socket = null;
        }
    }

    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 42069;
    public int id = 0;

    public TCP tcp;
    public UDP udp;

    private bool isConnected = false;

    private delegate void PacketHandler(Packet aPacket);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.Log("Client singleton already exists, destroying duplicate!");
            Destroy(this);
        }
    }

    private void Start() {
        
    }

    private void OnApplicationQuit() {
        Disconnect();
    }

    public void ConnectToServer() {

        tcp = new TCP();
        udp = new UDP();

        InitializeClientData();
        isConnected = true;
        tcp.Connect();
    }

    private void InitializeClientData() {
        packetHandlers = new Dictionary<int, PacketHandler>() {
            { (int)ServerPackets.WELCOME, ClientHandle.Welcome },
            { (int)ServerPackets.SPAWN_PLAYER, ClientHandle.SpawnPlayer },
            { (int)ServerPackets.PLAYER_POS, ClientHandle.PlayerPosition },
            { (int)ServerPackets.PLAYER_ROTATION, ClientHandle.PlayerRotation },
            { (int)ServerPackets.PLAYER_DISCONNECTED, ClientHandle.PlayerDisconnected },
            { (int)ServerPackets.PLAYER_HEALTH, ClientHandle.PlayerHealth },
            { (int)ServerPackets.PLAYER_RESPAWNED, ClientHandle.PlayerRespawned },
            { (int)ServerPackets.PLAYER_WEAPON_EQUIPPED, ClientHandle.WeaponEquipped },
            { (int)ServerPackets.PLAYER_WEAPON_FIRED, ClientHandle.WeaponFired },
            { (int)ServerPackets.PLAYER_WEAPON_RELOADED, ClientHandle.WeaponReloaded }
        };
        Debug.Log("[Client] - Initialized packets.");
    }

    private void Disconnect() {
        if (isConnected == true) {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnected from server.");
        }
    }
}
