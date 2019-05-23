using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnerController : MonoBehaviour
{
    [Header("Text field")]
    [SerializeField] private Text timeText;
    
    [Header("Creep Samples")]
    [SerializeField] private GameObject[] creepSamples;
    
    [Header("Time to spawn")]
    [SerializeField] private float spawnTime;
    
    [Header("Base Transform")]
    [SerializeField] private Transform target;
    
    private float _spawnTimer;
    private int _creepsCount;

    private void Start()
    {
        timeText.enabled = false;
        _spawnTimer = 0;
    }

    private void SpawnCreep()
    {
        int index = Random.Range(0, creepSamples.Length);
        GameObject currentCreep = creepSamples[index];
        
        var creep = PoolManager.GetObject(currentCreep.name, transform.position, Quaternion.identity);
        if (!creep)
            creep = Instantiate(currentCreep, transform.position, Quaternion.identity);

        creep.GetComponent<CreepController>().SetTarget(target);
        
        _spawnTimer = spawnTime;
        _creepsCount++;
        
        EventManager.Instance.Invoke("CreepSpawned", this, EventArgs.Empty);
    }

    public void StartWave()
    {
        StartCoroutine("Wave");
    }

    IEnumerator Wave()
    {
        while (_creepsCount < GameManager.Instance.CreepsPerWave)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0)
            {
                SpawnCreep();
            }

            yield return null;
        }

        _creepsCount = 0;
        EventManager.Instance.Invoke("WaveOver", this, EventArgs.Empty);
    }

    public void SetTimer(float time)
    {
        var coroutine = WaveTimer(time);
        StartCoroutine(coroutine);
    }

    IEnumerator WaveTimer(float time)
    {
        timeText.enabled = true;

        while (time > 0)
        {
            timeText.text = ((int)time).ToString();
            time -= Time.deltaTime;
            yield return null;
        }

        timeText.enabled = false;
    }
}
