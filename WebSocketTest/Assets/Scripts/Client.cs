using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Client : MonoBehaviour {

	public string serverIp = "192.168.3.213";
    public string serverPort = "14042";

    public bool connect = false;
    public bool ping = false;
	public bool connectAsync = true;

    bool connected = false;

    WebSocket webSocket;

	void Start () {
        connect = false;
        ping = false;
        webSocket = new WebSocket("ws://" + serverIp + ":" + serverPort + "/ping");
        webSocket.OnOpen += (sender, e) =>
        {
			connected = true;
            Debug.Log("Socket did open");
        };
		webSocket.OnClose += (object sender, CloseEventArgs e) => 
		{
			connected = false;
			Debug.Log("Socket did close");
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
			if (connect) {
				Debug.Log ("Trying to connect to server..");
				if (connectAsync)
					webSocket.ConnectAsync ();
				else
					webSocket.Connect ();
				Debug.Log ("Finished Connect() / ConnectAsync() call");
			} else
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
		if (connected) {
			if (connectAsync)
				webSocket.CloseAsync ();
			else
				webSocket.Close ();
		}
    }
}
