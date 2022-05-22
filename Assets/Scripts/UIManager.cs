using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField ipField;
    [SerializeField] private ServerNetworkManager server;

    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        ipField.interactable = false;
        connectUI.SetActive(false);
        if (!string.IsNullOrWhiteSpace(ipField.text))
        {
            ClientNetworkManager.Singleton.ip = ipField.text;

        }
        ClientNetworkManager.Singleton.Connect();
    }

    public void HostClicked()
    {
        ipField.placeholder.GetComponent<Text>().text = "you server omg wow";
        ipField.placeholder.color = Color.red;
        server.enabled = true;
    }

    public void BackToMain()
    {
        ipField.interactable = true;
        connectUI.SetActive(true);
    }

    public void Join()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.join);
        ClientNetworkManager.Singleton.Client.Send(message);
    }
}
