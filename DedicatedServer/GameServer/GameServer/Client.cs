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

        public Client(int aClientId) {
            id = aClientId;
            tcp = new TCP(id);
        }

        public class TCP {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP(int aId) {
                id = aId;
            }

            public void Connect(TcpClient aSocket) {
                socket = aSocket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

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

                    //TODO: handle data
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                } catch (Exception e) {
                    Console.WriteLine("[ERROR] - exception occured while receiving TCP data: " + e.ToString());

                    // TODO: disconnect client
                }
            } 
        }
    }
}
