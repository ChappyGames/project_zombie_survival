using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer {
    class Player {

        public int id;
        public string username;

        public Vector3 pos;
        public Quaternion rotation;

        private float moveSpeed = 5f / Constants.TICKS_PER_SECOND;
        private bool[] inputs;

        public Player (int aId, string aUsername, Vector3 aSpawnPos) {
            id = aId;
            username = aUsername;
            pos = aSpawnPos;
            rotation = Quaternion.Identity;

            inputs = new bool[4];
        }

        public void Update() {
            Vector2 lInputDirection = Vector2.Zero;
            if (inputs[0]) {
                lInputDirection.Y += 1;
            }

            if (inputs[1]) {
                lInputDirection.Y -= 1;
            }
            if (inputs[2]) {
                lInputDirection.X += 1;
            }
            if (inputs[3]) {
                lInputDirection.X -= 1;
            }

            Move(lInputDirection);
        }

        private void Move(Vector2 aInputDirection) {
            Vector3 lForward = new Vector3(0, 0, 1);//Vector3.Transform(new Vector3(0, 0, 1), rotation);
            Vector3 lRight = Vector3.Normalize(Vector3.Cross(lForward, new Vector3(0, 1, 0)));

            Vector3 lMoveDirection = lRight * aInputDirection.X + lForward * aInputDirection.Y;
            pos += lMoveDirection * moveSpeed;

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }

        public void SetInput(bool[] aInput, Quaternion aRotation) {
            inputs = aInput;
            rotation = aRotation;
        }
    }
}
