using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour { 

    public static void Welcome(Packet aPacket) {
        string lMessage = aPacket.ReadString();
        int lMyId = aPacket.ReadInt();

        Debug.Log($"[Client Handle] - Message from server: {lMessage}");
        Client.instance.id = lMyId;
        //TODO: send welcome received packet
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet aPacket) {
        int lId = aPacket.ReadInt();
        string lUsername = aPacket.ReadString();
        Vector3 lPosition = aPacket.ReadVector3();
        Quaternion lRotation = aPacket.ReadQuaternion();
        string lWeaponId = aPacket.ReadString();

        GameManager.Instance.SpawnPlayer(lId, lUsername, lPosition, lRotation, lWeaponId);
    }

    public static void EntityPosition(Packet aPacket) {
        int lType = aPacket.ReadInt();
        int lId = aPacket.ReadInt();
        Vector3 lPosition = aPacket.ReadVector3();

        EntityManager.Instance.GetEntity(lType, lId).Transform.position = lPosition;
    }

    public static void EntityRotation(Packet aPacket) {
        int lType = aPacket.ReadInt();
        int lId = aPacket.ReadInt();
        Quaternion lRotation = aPacket.ReadQuaternion();

        EntityManager.Instance.GetEntity(lType, lId).Transform.rotation = lRotation;
    }

    public static void PlayerDisconnected(Packet aPacket) {
        int lId = aPacket.ReadInt();

        Destroy(GameManager.players[lId].gameObject);
        GameManager.players.Remove(lId);
    }

    public static void EntityHealth(Packet aPacket) {
        int lType = aPacket.ReadInt();
        int lId = aPacket.ReadInt();
        float lHealth = aPacket.ReadFloat();

        EntityManager.Instance.GetEntity(lType, lId).SetHealth(lHealth);
    }

    public static void EntityRespawned(Packet aPacket) {
        int lType = aPacket.ReadInt();
        int lId = aPacket.ReadInt();

        EntityManager.Instance.GetEntity(lType, lId).OnRespawn();
    }

    public static void WeaponEquipped(Packet aPacket) {
        int lId = aPacket.ReadInt();
        string lWeaponId = aPacket.ReadString();

        GameManager.players[lId].SetWeapon(lWeaponId);
    }

    public static void WeaponFired(Packet aPacket) {
        int lId = aPacket.ReadInt();

        GameManager.players[lId].FireWeapon();
    }

    public static void WeaponReloaded(Packet aPacket) {
        int lId = aPacket.ReadInt();

        GameManager.players[lId].ReloadWeapon();
    }

}
