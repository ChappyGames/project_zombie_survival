using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

using ChappyGames.Client.Entities;
using ChappyGames.Client.Items;
using ChappyGames.Client.InventorySystem;

namespace ChappyGames.Client.Networking {

    public class ClientHandle : MonoBehaviour {

        public static void Welcome(Packet aPacket) {
            string lMessage = aPacket.ReadString();
            int lMyId = aPacket.ReadInt();

            Debug.Log($"[Client Handle] - Message from server: {lMessage}");
            Client.instance.id = lMyId;
            ClientSend.WelcomeReceived();

            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void EntitySpawned(Packet aPacket) {
            EntityManager.Instance?.SpawnEntityPacketHandler(aPacket);
        }

        public static void EntityPosition(Packet aPacket) {
            int lType = aPacket.ReadInt();
            int lId = aPacket.ReadInt();
            Vector3 lPosition = aPacket.ReadVector3();

            IEntity lEntity = EntityManager.Instance.GetEntity(lType, lId);
            if (lEntity != null) {
                lEntity.Position = lPosition;
            }
        }

        public static void EntityRotation(Packet aPacket) {
            // Quick fix for the issue where the server sends rotation packets for the client.
            int lType = aPacket.ReadInt();
            int lId = aPacket.ReadInt();

            if (lType == (int)EntityType.ENTITY_PLAYER && lId == Client.instance.id) {
                return;
            }
            Quaternion lRotation = aPacket.ReadQuaternion();

            IEntity lEntity = EntityManager.Instance.GetEntity(lType, lId);
            if (lEntity != null) {
                lEntity.Rotation = lRotation;
            }
        }

        public static void PlayerDisconnected(Packet aPacket) {
            int lId = aPacket.ReadInt();

            Destroy(EntityManager.Instance.GetEntity((int)EntityType.ENTITY_PLAYER, lId).gameObject);
            EntityManager.Instance.UnregisterEntity((int)EntityType.ENTITY_PLAYER, lId);
        }

        public static void EntityHealth(Packet aPacket) {
            int lType = aPacket.ReadInt();
            int lId = aPacket.ReadInt();
            float lHealth = aPacket.ReadFloat();

            EntityManager.Instance.GetMob(lType, lId).SetHealth(lHealth);
        }

        public static void EntityRespawned(Packet aPacket) {
            int lType = aPacket.ReadInt();
            int lId = aPacket.ReadInt();

            EntityManager.Instance.GetMob(lType, lId).OnRespawn();
        }

        public static void InventoryItemAdded(Packet aPacket) {
            int lMobType = aPacket.ReadInt();
            int lMobId = aPacket.ReadInt();
            int lItemType = aPacket.ReadInt();
            string lItemId = aPacket.ReadString();
            int lItemStack = aPacket.ReadInt();

            EntityManager.Instance.GetMob(lMobType, lMobId).Inventory.AddItem(new InventoryItem((ItemType)lItemType, lItemId, lItemStack));
        }

        public static void InventoryItemUsed(Packet aPacket) {
            int lMobType = aPacket.ReadInt();
            int lMobId = aPacket.ReadInt();
            int lItemType = aPacket.ReadInt();
            string lItemId = aPacket.ReadString();

            EntityManager.Instance.GetMob(lMobType, lMobId).Inventory.UseItem(new InventoryItem((ItemType)lItemType, lItemId));
        }

        public static void InventoryItemRemoved(Packet aPacket) {
            int lMobType = aPacket.ReadInt();
            int lMobId = aPacket.ReadInt();
            int lItemType = aPacket.ReadInt();
            string lItemId = aPacket.ReadString();
            int lItemStack = aPacket.ReadInt();

            EntityManager.Instance.GetMob(lMobType, lMobId).Inventory.RemoveItem(new InventoryItem((ItemType)lItemType, lItemId, lItemStack));
        }

        public static void WeaponEquipped(Packet aPacket) {
            int lId = aPacket.ReadInt();
            string lWeaponId = aPacket.ReadString();

            EntityManager.Instance.GetMob((int)EntityType.ENTITY_PLAYER, lId).Inventory.SetWeapon(lWeaponId);

        }

        public static void WeaponFired(Packet aPacket) {
            int lId = aPacket.ReadInt();

            EntityManager.Instance.GetPlayer((int)EntityType.ENTITY_PLAYER, lId).FireWeapon();
        }

        public static void WeaponReloaded(Packet aPacket) {
            int lId = aPacket.ReadInt();

            EntityManager.Instance.GetPlayer((int)EntityType.ENTITY_PLAYER, lId).ReloadWeapon();
        }

    }
}
