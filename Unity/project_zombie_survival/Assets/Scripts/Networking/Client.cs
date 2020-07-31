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
                    // TODO: disconnect
                    return;
                }

                byte[] lData = new byte[lByteLength];
                Array.Copy(receiveBuffer, lData, lByteLength);

                receivedData.Reset(HandleData(lData));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            } catch {
                // TODO: disconnect
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
                    //TODO: disconnect
                    return;
                }

                HandleData(lData);
            } catch {
                // TODO: disconnect
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
    }

    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 42069;
    public int id = 0;
    public TCP tcp;
    public UDP udp;

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
        tcp = new TCP();
        udp = new UDP();
    }

    public void ConnectToServer() {
        InitializeClientData();
        tcp.Connect();
    }

    private void InitializeClientData() {
        packetHandlers = new Dictionary<int, PacketHandler>() {
            { (int)ServerPackets.WELCOME, ClientHandle.Welcome },
            { (int)ServerPackets.UDP_TEST, ClientHandle.UDPTest }
        };
        Debug.Log("[Client] - Initialized packets.");
    }
}
