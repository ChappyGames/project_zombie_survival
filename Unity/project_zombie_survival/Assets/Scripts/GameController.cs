using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public Canvas respawnCanvas;

    private void Awake() {
        instance = this;
    }

}
