using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Assets.UDP_Connector
{
    public class UDPSender : MonoBehaviour {

        private bool _first = true;
        private int _port;

        public static string Ip = "192.168.0.100";
        public static string Port = "1111";

        public static bool Flag, Connected;

        public static IPEndPoint RemoteEndPoint;
        public static UdpClient Client;

        private static bool _isConnected;

        private void Update()
        {
            //If user pressed Connect and is not connected, then connects, else if is connected, stops the connection
            if (Connected)
            {
                if (_first)
                {
                    Debug.Log("Start UDP");
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

        private void Init()
        {
            RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Ip), _port);
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
                        //Debug.Log(message);
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
}
