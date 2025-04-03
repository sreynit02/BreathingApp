using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp.Server; // Import WebSocketSharp Github: https://github.com/sta/websocket-sharp
using WebSocketSharp;

public class UnityWebocketServer : MonoBehaviour
{
    private WebSocketServer wssServer;


    void Start()
    {
      
        wssServer = new WebSocketServer("ws://localhost:8000"); //replace with hmd ip address 

        wssServer.AddWebSocketService<BreathingDataHandler>("/data");

        wssServer.Start();

        if (wssServer.IsListening)
        {
            Debug.Log($"WebSocket server started.");
        }
    }
    private void OnApplicationQuit()
    {
        if (wssServer != null)
        {
            wssServer.Stop();
            wssServer = null;
        }
    }
    
}


public class BreathingDataHandler : WebSocketBehavior
{
    public static BreathingDataHandler Instance { get; private set; }

    public BreathingData breathingData;

    public BreathingDataHandler()
    {
        Instance = this;
        breathingData = new BreathingData();
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        breathingData = JsonUtility.FromJson<BreathingData>(e.Data);
        Debug.Log("Value Recieved: " + breathingData.value);
    }
}

[Serializable]
public class BreathingData
{
    public float value;
}