using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.Entities;

namespace ChappyGames.Client.Items {

    public enum ItemType {
        ITEM_NONE = 0,
        ITEM_BASIC = 1,
        ITEM_WEAPON = 2,
    }

    [CreateAssetMenu(menuName = "Project Zombie Survival/Items/New Item")]
    public class Item : ScriptableObject {

        [Header("Item Properties")]
        [SerializeField] private string id;
        [SerializeField] private string itemName;
        [SerializeField] private string itemDescription;
        [SerializeField] private float itemWeight;

        [SerializeField] private GameObject worldModel;

        public string ID => id;
        public virtual ItemType Type { get { return ItemType.ITEM_BASIC; } }
        public string ItemName => itemName;
        public string ItemDescription => itemDescription;
        public float ItemWeight => itemWeight;
        public GameObject WorldModel => worldModel;

        public virtual void Use(Mob aMob) {
            // Pass the mob that is using this item.
        }
    }
}
