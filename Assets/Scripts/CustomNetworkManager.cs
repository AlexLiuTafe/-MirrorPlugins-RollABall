using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
public class CustomNetworkManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);


        //*NOT USE BECAUSE WE HAVE A SPAWNER!!!
        /*//Spawn an instance of enemy onto client
        GameObject enemyPrefab = spawnPrefabs[0]; // reference it from network manager inspector
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 2, 0), Quaternion.identity);
        NetworkServer.Spawn(enemy);*/

    }


}
