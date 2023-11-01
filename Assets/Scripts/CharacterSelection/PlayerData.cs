using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
    public ulong ClientID;

    public PlayerData(ulong clientID)
    {
        ClientID = clientID;
    }

    public bool Equals(PlayerData other)
    {
        return ClientID == other.ClientID;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientID);
    }
}
