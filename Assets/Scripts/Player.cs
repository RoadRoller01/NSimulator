using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }


    private void Destroy()
    {
        list.Remove(Id);
    }
    void move(Vector3 position)
    {
        if (!IsLocal)
        {
            transform.position = position;
        }
    }

    public static void Spawn(ushort id, Vector3 position)
    {
        Player player;
        if (id == 1 && ClientNetworkManager.Singleton.Client.Id == 1)
        {

            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).AddComponent<HotCube>().GetComponent<Player>();
            player.IsLocal = true;
        }
        else if (id == ClientNetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).AddComponent<HotCube>().GetComponent<Player>();
            player.IsLocal = true;
        }
        else if (id == 1)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id}";
        player.Id = id;

        list.Add(id, player);
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetVector3());
    }



    [MessageHandler((ushort)ServerToClientId.playerMoved)]
    private static void MovePlayer(Message message)
    {
        Player player;
        list.TryGetValue(message.GetUShort(), out player);
        if (player != null)
        {
            player.move(message.GetVector3());
        }

    }
}