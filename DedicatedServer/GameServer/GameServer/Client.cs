using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer {
    class Client {

        public static int dataBufferSize = 4096;

        public int id;
        public TCP tcp;
        public UDP udp;

        public Client(int aClientId) {
            id = aClientId;
            tcp = new TCP(id);
            udp = new UDP(id);
        }

        public class TCP {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public TCP(int aId) {
                id = aId;
            }

            public void Connect(TcpClient aSocket) {
                socket = aSocket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receivedData = new Packet();
                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                ServerSend.Welcome(id, "Welcome to the server!");
            }

            public void SendData(Packet aPacket) {
                try {
                    if (socket != null) {
                        stream.BeginWrite(aPacket.ToArray(), 0, aPacket.Length(), null, null);
                    }
                } catch (Exception e) {
                    Console.WriteLine($"Error sending data to player {id} via TCP: {e}");
                }
            }

            private void ReceiveCallback(IAsyncResult aResult) {
                try {
                    int lByteLength = stream.EndRead(aResult);
                    if (lByteLength <= 0) {
                        //TODO: disconnect client
                        return;
                    }

                    byte[] lData = new byte[lByteLength];
                    Array.Copy(receiveBuffer, lData, lByteLength);

                    receivedData.Reset(HandleData(lData));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                } catch (Exception e) {
                    Console.WriteLine("[ERROR] - exception occured while receiving TCP data: " + e.ToString());

                    // TODO: disconnect client
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
                            Server.packetHandlers[lPacketId](id, lPacket);
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
            public IPEndPoint endPoint;

            private int id;

            public UDP(int aId) {
                id = aId;
            }

            public void Connect(IPEndPoint aEndPoint) {
                endPoint = aEndPoint;
                ServerSend.UDPTest(id);
            }

            public void SendData(Packet aPacket) {
                Server.SendUDPData(endPoint, aPacket);
            }

            public void HandleData(Packet aPacketData) {
                int lPacketLength = aPacketData.ReadInt();
                byte[] lPacketBytes = aPacketData.ReadBytes(lPacketLength);

                ThreadManager.ExecuteOnMainThread(() => {
                    using (Packet lPacket = new Packet(lPacketBytes)) {
                        int lPacketId = lPacket.ReadInt();
                        Server.packetHandlers[lPacketId](id, lPacket);
                    }
                });
            }
        }
    }
}
