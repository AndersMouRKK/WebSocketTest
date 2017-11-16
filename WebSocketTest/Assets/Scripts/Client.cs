using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Client : MonoBehaviour {

	public string serverIp = "192.168.3.213";
    public string serverPort = "4000";

    public bool connect = false;
    public bool ping = false;
    public bool send100Bytes = false;
	public bool connectAsync = true;

    bool connected = false;

    WebSocket webSocket;

	void Start () {
        connect = false;
        ping = false;
        webSocket = new WebSocket("ws://" + serverIp + ":" + serverPort + "/ping");
        webSocket.OnOpen += (sender, e) =>
        {
			connect = true;
			connected = true;
            Debug.Log("Socket did open");
        };
		webSocket.OnClose += (object sender, CloseEventArgs e) => 
		{
			connect = false;
			connected = false;
			Debug.Log("Socket did close" + e.Reason);
		};
        webSocket.OnMessage += (sender, e) =>
        {
            if(e.IsText)
            {
                Debug.Log("Server says: " + e.Data);
            }
            else if(e.IsBinary)
            {
                Debug.Log("Server send binary data of length: " + e.RawData.Length);
            }
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
				connect = false;
				Debug.Log ("Trying to connect to server..");
				if (connectAsync)
					webSocket.ConnectAsync ();
				else
					webSocket.Connect ();
				Debug.Log ("Finished Connect() / ConnectAsync() call");
			} else {
				connect = true;
				Debug.Log ("Trying to close connection");
				webSocket.CloseAsync ();
			}
        }
        if(connected)
        {
            if (ping)
            {
                ping = false;
                webSocket.Send("hello");
            }
            else if (send100Bytes)
            {
                send100Bytes = false;
                byte[] data = new byte[100];
                for(int i = 0; i < data.Length; i++)
                {
                    data[i] = (byte)i;
                }
                webSocket.Send(data);
            }
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
