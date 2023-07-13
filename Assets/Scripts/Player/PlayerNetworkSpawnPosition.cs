using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkSpawnPosition : NetworkBehaviour
{
    [SerializeField] private List<Vector3> _spawnPositions;
    public override void OnNetworkSpawn()
    {
        //owner client id can be cast into an int to determine player index
        int spawnIndex = (int)OwnerClientId;
        transform.position = _spawnPositions[spawnIndex];
    }
}
