using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawn : MonoBehaviour
{
    
    public GameObject[] Powerups;
    public Transform[] spawnPoints;
    int spawnPointsIndex;
    int powerupsIndex;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


   void SpawnPowerups()
    {
        spawnPointsIndex = Random.Range(0, spawnPoints.Length);
        powerupsIndex = Random.Range(0, Powerups.Length);
        Instantiate(Powerups[powerupsIndex], spawnPoints[spawnPointsIndex].position, spawnPoints[spawnPointsIndex].rotation);

    }

    public void StartSpawning()
    {
        InvokeRepeating("SpawnPowerups", 0, Random.Range(10,20));
    }

    public void SpawnAfterPickup()
    {
        Invoke("SpawnPowerups", Random.Range(5, 10));
    }

    
}
