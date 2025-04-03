//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using NativeWebSocket; //imported gitURLhttps://github.com/endel/NativeWebSocket by endel

//public class WebSocketClient : MonoBehaviour
//{
//    public static WebSocketClient Instance { get; private set; }
//    public BreathingData breathingData;

//    WebSocket websocket;

//    void Awake() 
//    {
//        Instance = this;
//    }

//    async void Start()
//    {
//        Debug.Log("Starting WebSocket client initialization...");
//        //websocket = new WebSocket("ws://10.74.246.62:8000"); //change to the IP address of the device that's runnning the server
//        websocket = new WebSocket("ws://localhost:8000");

//        websocket.OnOpen += () => Debug.Log("Connection open!");
//        websocket.OnError += (e) => Debug.LogError("Error: " + e);
//        websocket.OnClose += (e) => Debug.Log("Connection closed!");
//        websocket.OnMessage += (bytes) =>
//        {
//            var message = System.Text.Encoding.UTF8.GetString(bytes); //converts the byte array into a string using UTF-8 encoding which is in JSON format
//            breathingData = JsonUtility.FromJson<BreathingData>(message);
//            Debug.Log("data value received: " + breathingData.value);
//        };

//        await websocket.Connect(); //connect to server
//    }

//    void Update()
//    {
//#if !UNITY_WEBGL || UNITY_EDITOR
//        websocket.DispatchMessageQueue();
//#endif
//    }

//    async void OnApplicationQuit()
//    {
//        await websocket.Close();
//    }
//}


////serializable class used to store and handle the breathing effort data received from WebSocket server
//[Serializable]
//public class BreathingData
//{
//    //public string time;
//    public float value;
//}
