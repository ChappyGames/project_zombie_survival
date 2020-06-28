using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer {
    class Server {

        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        private static TcpListener tcpListener;

        public static void Start(int aMaxPlayer, int aPort) {
            MaxPlayers = aMaxPlayer;
            Port = aPort;

            Console.WriteLine("Starting server...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine("Server started on port: " + Port.ToString());
        }

        private static void TCPConnectCallback(IAsyncResult aResult) {
            TcpClient lClient = tcpListener.EndAcceptTcpClient(aResult);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine("Incoming connection from " + lClient.Client.RemoteEndPoint + "...");

            for (int i = 1; i <= MaxPlayers; i++) {
                if (clients[i].tcp.socket == null) {
                    clients[i].tcp.Connect(lClient);
                    return;
                }
            }

            Console.WriteLine(lClient.Client.RemoteEndPoint + " failed to connect: Server full!");
        }

        private static void InitializeServerData() {
            for (int i = 1; i <= MaxPlayers; i++) {
                clients.Add(i, new Client(i));
            }
        }
    }
}
