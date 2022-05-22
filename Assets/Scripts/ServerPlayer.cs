using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayer
{
    public static Dictionary<ushort, ServerPlayer> list = new Dictionary<ushort, ServerPlayer>();

    public ushort Id { get; private set; }
    public Vector3 position { get; private set; }


    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id)
    {
        foreach (ServerPlayer otherPlayer in list.Values)
            otherPlayer.SendSpawned(id);

        ServerPlayer player = new ServerPlayer();
        Debug.Log(id);
        player.position = new Vector3(Random.Range(1, 5), 0, Random.Range(1, 5));
        player.Id = id;

        player.SendSpawned();
        list.Add(id, player);
    }

    #region Messages
    private void SendSpawned()
    {
        ServerNetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        ServerNetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerSpawned)), toClientId);
    }

    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddVector3(position);
        return message;
    }

    [MessageHandler((ushort)ClientToServerId.join)]
    private static void Join(ushort fromClientId, Message message)
    {
        Spawn(fromClientId);
        ServerNetworkManager.Singleton.Server.Send(Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.joined), fromClientId);
    }

    [MessageHandler((ushort)ClientToServerId.move)]
    private static void Move(ushort fromClientId, Message message)
    {
        ServerNetworkManager.Singleton.Server.SendToAll(Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.playerMoved).AddUShort(fromClientId).AddVector3(message.GetVector3()));

    }
    #endregion
}