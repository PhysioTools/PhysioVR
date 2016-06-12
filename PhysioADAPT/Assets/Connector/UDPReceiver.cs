/*
    -----------------------
    based on UDP-Receive
    -----------------------
     [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
*/

using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Assets.Manager;

public class UDPReceiver : MonoBehaviour {

    private Thread _receiveThread;
    private UdpClient _client;
    private bool _isConnected;

    private int _port; 
    public static string Port = "1111";
    public static bool Flag, Connected;

    private string _message = "";
    private bool _first = true;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// create new thread to receive incoming messages.
    /// </summary>
    private void Init()
    {
        _receiveThread = new Thread(new ThreadStart(ReceiveData));
        _receiveThread.IsBackground = true;
        _receiveThread.Start();
        _isConnected = true;
    }


    /// <summary>
    /// receive thread
    /// </summary>
    private void ReceiveData()
    {
        _client = new UdpClient(_port);

        while (_isConnected)
        {
            try
            {
                // receive Bytes from 127.0.0.1
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Loopback, 0);

                byte[] data = _client.Receive(ref anyIP);

                //  UTF8 encoding in the text format.
                _message = Encoding.UTF8.GetString(data);
                Debug.Log("text: " + _message);
                DataManager.ParseData(_message);       
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }


    void Update()
    {
        if (Application.loadedLevel == 0)
        {
            if (Connected)
            {
                if (_first)
                {
                    _first = false;
                    _port = int.Parse(Port);
                    Init();
                    Flag = true;
                    print("Start Server");
                    Handheld.Vibrate();
                }
            }
            else
            {
                if (!_first)
                {
                    _first = true;
                    Flag = false;
                    _receiveThread.Abort();
                    _client.Close();
                    print("Stop Server");
                    Handheld.Vibrate();
                }
            }
        }
    }

    //get local ip address
    public static string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                localIP = ip.ToString();
        }

        return localIP;
    }

    void OnDisable()
    {
        if (_receiveThread != null)
        {
            _isConnected = false;
            _receiveThread.Abort();
            _client.Close();
        }
    }

    void OnApplicationQuit()
    {
        if (_receiveThread != null)
        {
            _isConnected = false;
            _receiveThread.Abort();
            _client.Close();
        }
    }
}
