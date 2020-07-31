using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer {
    class ThreadManager {

        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        /// <summary>
        /// Sets an action to be executed on the main thread.
        /// </summary>
        /// <param name="aAction">The action to be executed on the main thread.</param>
        public static void ExecuteOnMainThread(Action aAction) {
            if (aAction == null) {
                Console.WriteLine("[Thread Manager] - No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread) {
                executeOnMainThread.Add(aAction);
                actionToExecuteOnMainThread = true;
            }
        }

        /// <summary>
        /// Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.
        /// </summary>
        public static void UpdateMain() {
            if (actionToExecuteOnMainThread == true) {
                executeCopiedOnMainThread.Clear();
                lock (executeOnMainThread) {
                    executeCopiedOnMainThread.AddRange(executeOnMainThread);
                    executeOnMainThread.Clear();
                    actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < executeCopiedOnMainThread.Count; i++) {
                    executeCopiedOnMainThread[i]();
                }
            }
        }
    }
}
