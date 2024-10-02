using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnTime = 1.0f; 
    private float nextSpawnTime = 0f;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            Spawn();
            nextSpawnTime = Time.time + spawnTime;
        }
    }
    void Spawn()
    {
        float randomX = Random.Range(-6, 6);
        float randomY = Random.Range(-2, 5);
        Vector3 spawnPosition = new Vector3(randomX, randomY, transform.position.z);
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
