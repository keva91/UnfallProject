using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using SocketIO;

public class Player : MonoBehaviour {


    private SocketIOComponent socket;

    // Use this for initialization
    void Start () {

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

       

        socket.Emit("connexion");

        socket.On("message", (SocketIOEvent e) => {
            print("message ");
            Debug.Log("message");
            Debug.Log(e);
        });


    }


   


    void Update()
    {
        //socket.On("message", (SocketIOEvent e) => {
        //    print("message ");
        //    Debug.Log("message");
        //    Debug.Log(e);
        //});
    }

    void connectM(SocketIOEvent iOEvent)
    {
        print("connected ");
        print(iOEvent);
    }

}
