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

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
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

                //TODO: handle data
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            } catch {
                // TODO: disconnect
            }
        }
    }

    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 42069;
    public int id = 0;
    public TCP tcp;

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
    }

    public void ConnectToServer() {
        tcp.Connect();
    }
}
