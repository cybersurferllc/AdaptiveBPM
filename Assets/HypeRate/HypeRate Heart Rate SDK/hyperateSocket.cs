using NativeWebSocket;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class hyperateSocket : MonoBehaviour
{
    // Put your websocket Token ID here
    [SerializeField] private string websocketToken = "<Request your Websocket Token>"; //You don't have one, get it here https://www.hyperate.io/api
    [SerializeField] private string hyperateID = "internal-testing";
    // Textbox to display your heart rate in
    [SerializeField] private Text hyperateID_Text;
	// Websocket for connection with Hyperate
    private WebSocket websocket;
    public Action<float> BPMUpdated;
    async void Start()
    {
        websocketToken = Environment.GetEnvironmentVariable("HYPERATE_WEBSOCKET_KEY");
        hyperateID = Environment.GetEnvironmentVariable("HYPERATE_ID") ?? "internal-testing";
        websocket = new WebSocket("wss://app.hyperate.io/socket/websocket?token=" + websocketToken);
        hyperateID_Text.text = hyperateID;
        Debug.Log("Connect!");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
            SendWebSocketMessage();
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
        // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            var msg = JObject.Parse(message);

            if (msg["event"].ToString() == "hr_update")
            {
                // Change textbox text into the newly received Heart Rate (integer like "86" which represents beats per minute)
                var bpm = (string) msg["payload"]["hr"];
                BPMUpdated?.Invoke(Convert.ToInt32(bpm));
            }
        };

        // Send heartbeat message every 25seconds in order to not suspended the connection
        InvokeRepeating("SendHeartbeat", 1.0f, 25.0f);

        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Log into the "internal-testing" channel
            await websocket.SendText("{\"topic\": \"hr:"+hyperateID+"\", \"event\": \"phx_join\", \"payload\": {}, \"ref\": 0}");
        }
    }
    async void SendHeartbeat()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Send heartbeat message in order to not be suspended from the connection
            await websocket.SendText("{\"topic\": \"phoenix\",\"event\": \"heartbeat\",\"payload\": {},\"ref\": 0}");

        }
    }

    private async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }

}

public class HyperateResponse
{
    public string Event { get; set; }
    public string Payload { get; set; }
    public string Ref { get; set; }
    public string Topic { get; set; }
}
