using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplayer : MonoBehaviour {

    private int playerCount = 0;

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player " + playerCount++ + " connected from " + player.ipAddress + ":" + player.port);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
