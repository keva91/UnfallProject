using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Player : MonoBehaviour {

    public SocketIOComponent socket;

    
      

	// Use this for initialization
	void Start () {
        socket.Emit("Connexion");
		
	}
	
	
}
