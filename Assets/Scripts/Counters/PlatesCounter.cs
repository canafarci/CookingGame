using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectScriptableObject _plateKitchenObjectSO;
    private float _plateSpawnTimer;
    private int _platesSpawnedCount;
    private const float PLATE_SPAWN_TIME = 4f;
    private const int PLATE_SPAWN_MAX_COUNT = 4;
    //events
    public event EventHandler<OnPlateSpawnedEventArgs> OnPlateSpawned;
    private void Update()
    {
        _plateSpawnTimer += Time.deltaTime;
        if (_plateSpawnTimer > PLATE_SPAWN_TIME)
        {
            _plateSpawnTimer = 0f;
            //spawn
            if (_platesSpawnedCount < PLATE_SPAWN_MAX_COUNT)
            {
                _platesSpawnedCount++;
                OnPlateSpawned?.Invoke(this, new OnPlateSpawnedEventArgs { Change = OnPlateSpawnedEventArgs.CountChangeType.Increase });
            }
        }
    }
    public override void Interact(Player player)
    {
        //player is empty handed and there are plates on the table
        if (!player.HasKitchenObject() && _platesSpawnedCount > 0)
        {
            _platesSpawnedCount--;
            KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);
            OnPlateSpawned?.Invoke(this, new OnPlateSpawnedEventArgs { Change = OnPlateSpawnedEventArgs.CountChangeType.Decrease });
        }
    }
}

public class OnPlateSpawnedEventArgs : EventArgs
{
    public enum CountChangeType { Increase, Decrease };
    public CountChangeType Change;
}