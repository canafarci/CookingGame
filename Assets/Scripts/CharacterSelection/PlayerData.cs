using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
    public ulong ClientID;
    public int ColorIndex;

    public PlayerData(ulong clientID, int colorIndex)
    {
        ClientID = clientID;
        ColorIndex = colorIndex;
    }

    public bool Equals(PlayerData other)
    {
        return ClientID == other.ClientID && ColorIndex == other.ColorIndex;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientID);
        serializer.SerializeValue(ref ColorIndex);
    }
}
