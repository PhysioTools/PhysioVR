using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System;

public class UDPSender : MonoBehaviour {

    private bool _first = true;

    private string _iP;
    private int _port;
    private static string _message;

    public static string IP = "192.168.0.100";
    public static string Port = "1112";

    public static bool Flag, Connected;

    public static IPEndPoint RemoteEndPoint;
    public static UdpClient Client;

    private static bool _isConnected;

    private void Update()
    {
        if (Application.loadedLevel == 0)
        {
            //If user pressed Connect and is not connected, then connects, else if is connected, stops the connection
            if (Connected)
            {
                if (_first)
                {
                    Debug.Log("Start UDP");
                    _iP = IP;
                    _port = int.Parse(Port);
                    Init();
                    Flag = true;
                    Handheld.Vibrate();
                    _first = false;
                }
            }
            else
            {
                if (Flag)
                {
                    if (!_first)
                    {
                        Flag = false;
                        _first = true;
                        Client.Close();
                        Debug.Log("Stop UDP");
                        Handheld.Vibrate();
                    }
                }
            }
        }
    }

    private void Init()
    {
        RemoteEndPoint = new IPEndPoint(IPAddress.Parse(_iP), _port);
        Client = new UdpClient();
        _isConnected = true;
        Debug.Log("Connected");
    }
    
    public static void SendStringMessage(string message)
    {
        if (_isConnected)
        {
            try
            {
                if (message != "")
                {
                    // UTF8 encoding to binary format.
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    
                    // Send the message to the remote client.
                    Client.Send(data, data.Length, RemoteEndPoint);
                }
            }

            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
    }
    
    void OnApplicationQuit()
    {
        if (Client != null)
        {
            _isConnected = false;
            Client.Close();
        }
    }
}
