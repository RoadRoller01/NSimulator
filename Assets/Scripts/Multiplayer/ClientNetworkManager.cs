using System;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

public class ClientNetworkManager : MonoBehaviour
{
    private static ClientNetworkManager _singleton;
    public static ClientNetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(ClientNetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client { get; private set; }

    [SerializeField] public string ip;
    [SerializeField] private ushort port;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        Client.Tick();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.Join();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        ip = "127.0.0.1";
        UIManager.Singleton.BackToMain();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Id].gameObject);
    }


    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }
}
