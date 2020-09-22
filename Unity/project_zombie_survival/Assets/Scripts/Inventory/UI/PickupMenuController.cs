using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMenuController : MonoBehaviour {

    [SerializeField] private Text pickupText;

    public void Prompt(string aPickupMessage) {
        gameObject.SetActive(true);
        pickupText.text = "Pickup " + aPickupMessage;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
