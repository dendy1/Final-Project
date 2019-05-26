using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[Serializable]
public class Wave
{
    [Header("Choose creeps for wave")]
    [SerializeField] private CreepForWave[] creepsForSpawn;
    
    [Header("Set spawn time for creeps")]
    [SerializeField] private float spawnTime;

    public CreepForWave[] CreepsForSpawn => creepsForSpawn;
    public float SpawnTime => spawnTime;

    public int CreepsCount
    {
        get
        {
            int res = 0;
            foreach (var creep in creepsForSpawn)
            {
                res += creep.Count;
            }

            return res;
        }
    }
}

[Serializable]
public class CreepForWave
{
    public GameObject Sample;
    public int Count;
}
