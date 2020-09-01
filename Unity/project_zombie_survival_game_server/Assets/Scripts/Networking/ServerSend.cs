using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChappyGames.Entities; // Should move functions that use this library into their respective classes

namespace ChappyGames.Networking {

    public class ServerSend {

        private static void SendTCPData(int aToClient, Packet aPacket) {
            aPacket.WriteLength();
            Server.clients[aToClient].tcp.SendData(aPacket);
        }

        private static void SendUDPData(int aToClient, Packet aPacket) {
            aPacket.WriteLength();
            Server.clients[aToClient].udp.SendData(aPacket);
        }

        private static void SendTCPDataToAll(Packet aPacket) {
            aPacket.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                Server.clients[i].tcp.SendData(aPacket);
            }
        }

        private static void SendTCPDataToAll(int aExceptClient, Packet aPacket) {
            aPacket.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                if (i != aExceptClient) {
                    Server.clients[i].tcp.SendData(aPacket);
                }
            }
        }

        private static void SendUDPDataToAll(Packet aPacket) {
            aPacket.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                Server.clients[i].udp.SendData(aPacket);
            }
        }

        private static void SendUDPDataToAll(int aExceptClient, Packet aPacket) {
            aPacket.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++) {
                if (i != aExceptClient) {
                    Server.clients[i].udp.SendData(aPacket);
                }
            }
        }

        #region Packets
        public static void Welcome(int aToClient, string aMessage) {
            using (Packet lPacket = new Packet((int)ServerPackets.WELCOME)) {
                lPacket.Write(aMessage);
                lPacket.Write(aToClient);

                SendTCPData(aToClient, lPacket);
            }
        }

        public static void SpawnPlayer(int aToClient, Player aPlayer) {
            using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_SPAWN)) {
                lPacket.Write(aPlayer.ID);
                lPacket.Write(aPlayer.username);
                lPacket.Write(aPlayer.transform.position);
                lPacket.Write(aPlayer.transform.rotation);

                SendTCPData(aToClient, lPacket);
            }
        }

        public static void SpawnEntity(int aToClient, Entity aEntity) {
            using (Packet lPacket = new Packet((int)ServerPackets.ENTITY_SPAWN)) {
                lPacket.Write((int)aEntity.Type);
                lPacket.Write(aEntity.ID);
                lPacket.Write(aEntity.transform.position);
                lPacket.Write(aEntity.transform.rotation);

                SendTCPData(aToClient, lPacket);
            }
        }

        public static void EntityPosition(Entity aEntity) {
            using (Packet lPacket = new Packet((int)ServerPackets.ENTITY_POS)) {
                lPacket.Write((int)aEntity.Type);
                lPacket.Write(aEntity.ID);
                lPacket.Write(aEntity.transform.position);

                SendUDPDataToAll(lPacket);
            }
        }

        public static void EntityRotation(Entity aEntity) {
            using (Packet lPacket = new Packet((int)ServerPackets.ENTITY_ROTATION)) {
                lPacket.Write((int)aEntity.Type);
                lPacket.Write(aEntity.ID);
                lPacket.Write(aEntity.transform.rotation);

                SendUDPDataToAll(lPacket);
            }
        }

        public static void PlayerDisconnected(int aPlayer) {
            using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_DISCONNECTED)) {
                lPacket.Write(aPlayer);

                SendTCPDataToAll(lPacket);
            }
        }

        public static void EntityHealth(Mob aEntity) {
            using (Packet lPacket = new Packet((int)ServerPackets.ENTITY_HEALTH)) {
                lPacket.Write((int)aEntity.Type);
                lPacket.Write(aEntity.ID);
                lPacket.Write(aEntity.Health);

                SendTCPDataToAll(lPacket);
            }
        }

        public static void EntityRespawned(Entity aEntity) {
            using (Packet lPacket = new Packet((int)ServerPackets.ENTITY_RESPAWNED)) {
                lPacket.Write((int)aEntity.Type);
                lPacket.Write(aEntity.ID);

                SendTCPDataToAll(lPacket);
            }
        }

        public static void WeaponEquipped(Player aPlayer) {
            using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_WEAPON_EQUIPPED)) {
                lPacket.Write(aPlayer.ID);
                lPacket.Write(aPlayer.attack.CurrentWeapon.ID);

                SendTCPDataToAll(lPacket);
            }
        }

        public static void WeaponFire(Entity aEntity) {
            using (Packet lPacket = new Packet((int)ServerPackets.ENTITY_ATTACK)) {
                lPacket.Write((int)aEntity.Type);
                lPacket.Write(aEntity.ID);

                //TODO: At some point, we shouldn't have to send this packet to the client firing their weapon since it should be handled on the client side.
                SendTCPDataToAll(lPacket);
            }
        }

        public static void WeaponReload(Player aPlayer) {
            using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_WEAPON_RELOADED)) {
                lPacket.Write(aPlayer.ID);

                //TODO: At some point, we shouldn't have to send this packet to the client reloading their weapon since it should be handled on the client side.
                SendTCPDataToAll(lPacket);
            }
        }
        #endregion
    }
}
