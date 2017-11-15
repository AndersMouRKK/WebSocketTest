using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Server : MonoBehaviour {

    public string serverIp = "127.0.0.1";
    public string serverPort = "10000";

    internal class Ping : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.Log("Client says: " + e.Data);
            Send("pong");
        }
    }

    WebSocketServer server;

	void Start () {
        server = new WebSocketServer("ws://" + serverIp + ":" + serverPort);
        server.AddWebSocketService<Ping>("/ping");
        server.Start();
        Debug.Log("Server started");
	}

    void OnApplicationQuit()
    {
        server.Stop();
    }
}
