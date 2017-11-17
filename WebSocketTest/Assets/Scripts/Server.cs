using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Server : MonoBehaviour {

    public int serverPort = 4000;

    public bool sendRandomString = false;
    public bool sendRandomBytePacket = false;

    internal class Ping : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsText)
            {
                Debug.Log("Client says: " + e.Data);
                Send("pong - " + e.Data);
            }
            else if (e.IsBinary)
            {
                Debug.Log("Client sent a binary message of length: " + e.RawData.Length);
                byte[] data = new byte[50];
                for(int i = 0; i < data.Length; i++)
                {
                    data[i] = (byte)i;
                }
                Send(data);
            }
            
        }
        protected override void OnClose(CloseEventArgs e)
        {
            Debug.Log("Connection to Ping closed: " + e.Reason);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            Debug.LogWarning("Ping error: " + e.Message);
        }
        protected override void OnOpen()
        {
            Debug.Log("Connection to Ping opened");   
        }
    }

    WebSocketServer server;

	void Start () {
        server = new WebSocketServer(serverPort);
        server.AddWebSocketService<Ping>("/ping");
        server.Start();
        Debug.Log("Server asked to start");
	}

    private void Update()
    {
        if(server != null)
        {
            if(sendRandomString)
            {
                sendRandomString = false;
                int n = Random.Range(0, 3);
                string str = n == 0 ? "Hi There!" : n == 1 ? "Oh hello" : "Okay now";
                server.WebSocketServices["/ping"].Sessions.Broadcast(str);
            }
            else if(sendRandomBytePacket)
            {
                sendRandomBytePacket = false;
                int n = Random.Range(40, 150);
                byte[] buffer = new byte[n];
                for(int i = 0; i < n; i++)
                {
                    buffer[i] = (byte)i;
                }
                server.WebSocketServices["/ping"].Sessions.Broadcast(buffer);
            }
        }
    }

    void OnApplicationQuit()
    {
        server.Stop();
    }

    private void OnGUI()
    {
        if(server != null)
        {
            GUI.Label(new Rect(50, 50, 200, 50), "Server's listening: " + server.IsListening);
            GUI.Label(new Rect(50, 120, 200, 50), "Server's secure: " + server.IsSecure);
        }
        
    }
}
