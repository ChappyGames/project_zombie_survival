using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChappyGames.Client.Entities;
using ChappyGames.Client.Networking;

public class GameManager : Singleton<GameManager> {

    public static Dictionary<int, Player> players = new Dictionary<int, Player>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    
}
