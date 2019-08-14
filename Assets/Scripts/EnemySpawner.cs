using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1.0f;

    public override void OnStartServer() // Run when the server start
    {
        //                      Beginning of the spawn time , and when it will happen again
        InvokeRepeating("SpawnEnemy", this.spawnInterval, this.spawnInterval);

    }
    void SpawnEnemy()
    {
        //                                  random X Axis            ,  Y axis                  ,random Z Axis
        Vector3 spawnPosition = new Vector3(Random.Range(-4.0f, 4.0f), this.transform.position.y,Random.Range(-4f,4f));
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(enemy);
        Destroy(enemy, 10);
    }
}
