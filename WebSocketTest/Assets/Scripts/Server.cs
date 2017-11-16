﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Server : MonoBehaviour {

    public string serverIp = "192.168.3.213";
    public string serverPort = "4000";

    internal class Ping : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsText)
            {
                Debug.Log("Client says: " + e.Data);
                Send("pong");
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
        server = new WebSocketServer("ws://" + serverIp + ":" + serverPort);
        //server = new WebSocketServer(14042, true);
        //string certPath = Application.dataPath + "/TestCert2.pfx";
        //server.SslConfiguration.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath);
        server.AddWebSocketService<Ping>("/ping");
        server.Start();
        Debug.Log("Server asked to start");
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
