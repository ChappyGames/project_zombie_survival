using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject playerModel;

    public int id;
    public string username;

    public Transform ModelTransform { get { return playerModel.transform; } }
}
