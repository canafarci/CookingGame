using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

public class PlayerDataHolder : NetworkBehaviour
{
    [SerializeField] private PlayerColorLookup _playerColorLookup;
    private NetworkList<PlayerData> _playerDataList;
    public event EventHandler<PlayerDataListChangedArgs> OnPlayerDataListChanged;

    private void Awake()
    {
        _playerDataList = new();
        _playerDataList.OnListChanged += PlayerDataList_OnPlayerDataListChanged;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void PlayerDataList_OnPlayerDataListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        int playerCount = _playerDataList.Count;
        ulong clientID = _playerDataList[playerCount - 1].ClientID;

        OnPlayerDataListChanged?.Invoke(this,
                                        new PlayerDataListChangedArgs
                                        {
                                            PlayerCount = playerCount,
                                            PlayerIndex = _playerDataList.Count - 1
                                        });
    }

    public void AddPlayerDataToList(ulong clientId)
    {
        int unusedColorIndex = GetUnusedColorIndex();
        PlayerData playerData = new(clientId, unusedColorIndex);
        _playerDataList.Add(playerData);
    }

    public void ChangePlayerColor(int colorIndex, ulong senderClientID)
    {
        if (IsColorUsed(colorIndex)) return;

        int senderIndex = GetPlayerIndex(senderClientID);
        PlayerData playerData = _playerDataList[senderIndex];
        playerData.ColorIndex = colorIndex;

        _playerDataList[senderIndex] = playerData;
    }

    private bool IsColorUsed(int colorIndex)
    {
        bool isUsed = false;

        bool[] usedColors = CreateUsedColorsArray();

        isUsed = usedColors[colorIndex];
        return isUsed;
    }

    private int GetUnusedColorIndex()
    {
        int unusedIndex = -1;

        bool[] usedColors = CreateUsedColorsArray();

        for (int i = 0; i < usedColors.Length; i++)
        {
            if (!usedColors[i])
            {
                unusedIndex = i;
                break;
            }
        }

        return unusedIndex;
    }

    private bool[] CreateUsedColorsArray()
    {
        int colorCount = _playerColorLookup.GetColorsCount();
        bool[] usedColors = new bool[colorCount];

        foreach (PlayerData playerData in _playerDataList)
        {
            int usedColorIndex = playerData.ColorIndex;
            usedColors[usedColorIndex] = true;
        }

        return usedColors;
    }

    public int GetPlayerIndex(ulong senderClientID)
    {
        int index = -1;

        for (int i = 0; i < _playerDataList.Count; i++)
        {
            PlayerData playerData = _playerDataList[i];
            if (playerData.ClientID == senderClientID)
            {
                index = i;
            }
        }

        return index;
    }

    public Color GetPlayerColor(ulong senderClientID)
    {
        int playerIndex = GetPlayerIndex(senderClientID);
        int colorIndex = _playerDataList[playerIndex].ColorIndex;

        Color color = _playerColorLookup.GetColorByIndex(colorIndex);

        return color;
    }

    public int GetPlayerColorIndex(int playerIndex) => _playerDataList[playerIndex].ColorIndex;

    public bool IsColorSelectedByClient(ulong clientID, int colorIndex)
    {
        bool isSelected = false;
        foreach (PlayerData playerData in _playerDataList)
        {
            if (playerData.ClientID == clientID && playerData.ColorIndex == colorIndex)
            {
                isSelected = true;
                break;
            }
        }
        return isSelected;
    }
}

public class PlayerDataListChangedArgs : EventArgs
{
    public int PlayerCount;
    public int PlayerIndex;
}