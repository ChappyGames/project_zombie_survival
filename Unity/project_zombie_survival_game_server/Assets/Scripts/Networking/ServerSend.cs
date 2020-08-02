using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        using (Packet lPacket = new Packet((int)ServerPackets.SPAWN_PLAYER)) {
            lPacket.Write(aPlayer.id);
            lPacket.Write(aPlayer.username);
            lPacket.Write(aPlayer.transform.position);
            lPacket.Write(aPlayer.transform.rotation);
            lPacket.Write(aPlayer.attack.CurrentWeapon.ID);

            SendTCPData(aToClient, lPacket);
        }
    }

    public static void PlayerPosition(Player aPlayer) {
        using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_POS)) {
            lPacket.Write(aPlayer.id);
            lPacket.Write(aPlayer.transform.position);

            SendUDPDataToAll(lPacket);
        }
    }

    public static void PlayerRotation(Player aPlayer) {
        using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_ROTATION)) {
            lPacket.Write(aPlayer.id);
            lPacket.Write(aPlayer.transform.rotation);

            SendUDPDataToAll(aPlayer.id, lPacket);
        }
    }

    public static void PlayerDisconnected(int aPlayer) {
        using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_DISCONNECTED)) {
            lPacket.Write(aPlayer);

            SendTCPDataToAll(lPacket);
        }
    }

    public static void PlayerHealth(Player aPlayer) {
        using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_HEALTH)) {
            lPacket.Write(aPlayer.id);
            lPacket.Write(aPlayer.health);

            SendTCPDataToAll(lPacket);
        }
    }

    public static void PlayerRespawned(Player aPlayer) {
        using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_RESPAWNED)) {
            lPacket.Write(aPlayer.id);

            SendTCPDataToAll(lPacket);
        }
    }

    public static void WeaponEquipped(Player aPlayer) {
        using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_WEAPON_EQUIPPED)) {
            lPacket.Write(aPlayer.id);
            lPacket.Write(aPlayer.attack.CurrentWeapon.ID);

            SendTCPDataToAll(lPacket);
        }
    }

    public static void WeaponFire(Player aPlayer) {
        using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_WEAPON_FIRED)) {
            lPacket.Write(aPlayer.id);

            //TODO: At some point, we shouldn't have to send this packet to the client firing their weapon since it should be handled on the client side.
            SendTCPDataToAll(lPacket);
        }
    }

    public static void WeaponReload(Player aPlayer) {
        using (Packet lPacket = new Packet((int)ServerPackets.PLAYER_WEAPON_RELOADED)) {
            lPacket.Write(aPlayer.id);

            //TODO: At some point, we shouldn't have to send this packet to the client reloading their weapon since it should be handled on the client side.
            SendTCPDataToAll(lPacket);
        }
    }
    #endregion
}
