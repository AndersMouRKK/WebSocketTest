using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Client : MonoBehaviour {

    public string serverIp = "127.0.0.1";
    public string serverPort = "10000";

    public bool connect = false;
    public bool ping = false;

    bool connected = false;

    WebSocket webSocket;

	void Start () {
        connect = false;
        ping = false;
        webSocket = new WebSocket("ws://" + serverIp + ":" + serverPort);
        webSocket.OnOpen += (sender, e) =>
        {
            Debug.Log("Socket did open");
        };
        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Server says: " + e.Data);
        };
        webSocket.OnError += (sender, e) =>
        {
            Debug.LogWarning("Socket error: " + e.Message);
        };
	}

    void Update()
    {
        if(connected != connect)
        {
            connected = connect;
            if (connect)
                webSocket.Connect();
            else
                webSocket.Close();
        }
        if(connected && ping)
        {
            ping = false;
            webSocket.Send("hello");
        }
    }

    void OnApplicationQuit()
    {
        if (connected)
            webSocket.Close();
    }
}
