using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace WebWork
{
    public static class WebSocket
    {
        private static ClientWebSocket WS = null;
        public static Uri ServerURI = null;

        public static void Connect(string uri)
        {
            ServerURI = new Uri(uri);
            Debug.Log(uri + " " + ServerURI.ToString());

            Task task = Task.Run(async () => { await ConnectAsync(ServerURI); });
        }
        private static async Task ConnectAsync(Uri uri)
        {
            //CancellationTokenSource source = new CancellationTokenSource(); - never used?
            using (var ws = new ClientWebSocket())
            {
                WS = ws;
                try
                {
                    await ws.ConnectAsync(uri, CancellationToken.None);
                    //Debug.Log("Attempting to connect to server");
                }
                catch (Exception e)
                {
                    Debug.Log("Could not connect to server: " + uri.ToString() + " " + e.Message);
                }

                byte[] buffer = new byte[256];
                while (ws.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    }
                    else
                    {
                        HandleMessage(buffer, result);
                    }
                }
                Debug.Log("Connection Terminated: " + uri.ToString());
            }
        }

        public static void SendMessage(string msg)
        {
            Task task = Task.Run(async () => { await SendMessageAsync(msg); });
            task.Wait();
        }

        private async static Task SendMessageAsync(string msg)
        {
            var ws = WS;
            if(ws.State != WebSocketState.Open)
            {
                return;
            }
            
            try
            {
                ArraySegment<byte> bytesToSend = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(msg));
                await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Debug.Log("Could not send message to server: " + e.Message);
            }
        }

        private static void HandleMessage(ArraySegment<byte> bytesReceived, WebSocketReceiveResult result)
        {
            string msg = System.Text.Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);
            Debug.Log("Server Response: " + msg);
        }
    }

}