using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer {
    class GameLogic {

        public static void Update() {

            foreach (Client lClient in Server.clients.Values) {
                lClient.player?.Update();
            }

            ThreadManager.UpdateMain();
        }
    }
}
