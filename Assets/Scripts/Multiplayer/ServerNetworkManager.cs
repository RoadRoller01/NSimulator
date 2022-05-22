using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

public class ServerNetworkManager : MonoBehaviour
{
    private static ServerNetworkManager _singleton;
    public static ServerNetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(ServerNetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Server Server { get; private set; }
    public bool isServer = false;

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        isServer = true;
        Application.targetFrameRate = 60;

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }

    private void FixedUpdate()
    {
        Server.Tick();
    }

    private void OnApplicationQuit()
    {
        if (isServer)
        {
            Server.Stop();
        }
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        ServerPlayer.list.Remove(e.Id);
    }
}
