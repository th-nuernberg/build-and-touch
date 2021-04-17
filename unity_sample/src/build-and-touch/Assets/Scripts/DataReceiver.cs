using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Sockets;

public class DataReceiver : Singleton<DataReceiver>
{
    public string address = "127.0.0.1";
    public int port = 8181;
        
    public Hand leftHand; 
    public Hand rightHand;

    private static TcpListener Listener { get; set; }
    private Task listenTask;

    private async void Start()
    {
        Listener = new TcpListener(IPAddress.Parse(address), port);
        await Listen();
    }

    private async Task Listen()
    {
        Listener.Start();
        
        while (true)
        {
            Debug.Log("Waiting for client...");
            
            var client = await Listener.AcceptTcpClientAsync();
            Debug.Log("Client connected. Waiting for data.");
            
            var stream = client.GetStream();
            
            try
            {
                while (client.Connected)
                {
                    var lengthB = new byte[4];
                    await stream.ReadAsync(lengthB, 0, 4);
                    var length = BitConverter.ToUInt32(lengthB, 0);

                    var buffer = new byte[length];
                    var bytesRead = 0;
                    
                    while (bytesRead < length)
                    {
                        bytesRead += await stream.ReadAsync(buffer, bytesRead, buffer.Length - bytesRead);
                    }
                    
                    var hands = Message.Parser.ParseFrom((buffer));

                    foreach (var hand in hands.Hands)
                    {
                        var landmarks = hand.Landmarks.ToArray();

                        if (hand.Hand == HandData.Types.HandType.RightHand)
                        {
                            rightHand.Landmarks = landmarks;
                            if(!rightHand.IsActive) rightHand.GenerateHand();
                        }
                        else
                        {
                            leftHand.Landmarks = landmarks;
                            if(!leftHand.IsActive) leftHand.GenerateHand();
                        }
                    }

                    await Task.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            
            Debug.Log("Connection closed.");  
            
        }
        
    }
}