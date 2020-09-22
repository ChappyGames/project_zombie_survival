using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.Entities;
using ChappyGames.Client.InventorySystem;

namespace ChappyGames.Client.Networking {

    public class ClientSend : MonoBehaviour {

        private static void SendTCPData(Packet aPacket) {
            aPacket.WriteLength();
            Client.instance.tcp.SendData(aPacket);
        }

        private static void SendUDPData(Packet aPacket) {
            aPacket.WriteLength();
            Client.instance.udp.SendData(aPacket);
        }

        #region Packets
        public static void WelcomeReceived() {
            using (Packet lPacket = new Packet((int)ClientPackets.WELCOME_RECEIVED)) {
                lPacket.Write(Client.instance.id);
                lPacket.Write(UIManager.instance.usernameField.text);

                SendTCPData(lPacket);
            }
        }

        public static void PlayerMovement(bool[] aInputs) {
            using (Packet lPacket = new Packet((int)ClientPackets.PLAYER_MOVEMENT)) {
                lPacket.Write(aInputs.Length);
                foreach (bool lInput in aInputs) {
                    lPacket.Write(lInput);
                }

                lPacket.Write(EntityManager.Instance.GetEntity((int)EntityType.ENTITY_PLAYER, Client.instance.id).Rotation);

                SendUDPData(lPacket);
            }
        }

        public static void PlayerAttack(Vector3 aFacing) {
            using (Packet lPacket = new Packet((int)ClientPackets.PLAYER_ATTACK)) {
                lPacket.Write(aFacing);

                SendTCPData(lPacket);
            }
        }

        public static void PlayerItemUsed(InventoryItem aItem) {
            using (Packet lPacket = new Packet((int)ClientPackets.PLAYER_ITEM_USED)) {
                lPacket.Write((int)aItem.type);
                lPacket.Write(aItem.itemId);
                lPacket.Write(aItem.stack);

                SendTCPData(lPacket);
            }
        }

        public static void PlayerAction() {
            using (Packet lPacket = new Packet((int)ClientPackets.PLAYER_ACTION)) {
                SendTCPData(lPacket);
            }
        }
        #endregion
    }
}
