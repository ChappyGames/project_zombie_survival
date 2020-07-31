using System;
using System.Threading;

namespace GameServer {
    class Program {

        private static bool isRunning = false;
        static void Main(string[] args) {
            Console.Title = "Game Server";
            isRunning = true;

            Thread lMainThread = new Thread(new ThreadStart(MainThread));
            lMainThread.Start();

            Server.Start(50, 42069);
        }

        private static void MainThread() {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SECOND} ticks per second.");
            DateTime lNextLoop = DateTime.Now;

            while (isRunning) {
                while (lNextLoop < DateTime.Now) {
                    GameLogic.Update();

                    lNextLoop = lNextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (lNextLoop > DateTime.Now) {
                        Thread.Sleep(lNextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
