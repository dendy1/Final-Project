using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnerController : MonoBehaviour
{
    [Header("Text field")]
    [SerializeField] private Text tableText;

    [Header("Waves Settings")] 
    [SerializeField] private Wave[] waves;
    [SerializeField] private GameObject bossSample;
    
    [Header("Base Transform")]
    [SerializeField] private Transform target;
    
    private float _spawnTimer;
    private int _currentWaveIndex;
    private bool _bossFighting;

    public bool WavesOver { get; private set; }
    public bool WaveIsOn { get; private set; }
    
    private void Start()
    {
        tableText.enabled = false;
        _spawnTimer = 0;
    }

    private void SpawnCreep(GameObject creepSample)
    {
        var spawnPosition = new Vector3(transform.position.x, 0, transform.position.z);
        var creepObj = GameManager.Instance.SpawnObject(creepSample, spawnPosition, Quaternion.identity);
        creepObj.GetComponent<CreepController>().SetTarget(target.position);
        EventManager.Instance.Invoke("CreepSpawned", this, EventArgs.Empty);
    }

    public void StartWave()
    {
        StopCoroutine("StartNewWave");
        StartCoroutine("StartNewWave");
    }
    
    public void StartFinalWave()
    {
        StopCoroutine("StartNewBossWave");
        StartCoroutine("StartNewBossWave");
    }

    IEnumerator StartNewWave()
    {
        while (_currentWaveIndex >= waves.Length)
            yield return null;

        WaveIsOn = true;
        Wave currentWave = waves[_currentWaveIndex];
        
        int creepType = 0;

        while (currentWave.CreepsCount > 0)
        {
            _spawnTimer -= Time.deltaTime;
            
            if (_spawnTimer <= 0)
            {
                CreepForWave creep = currentWave.CreepsForSpawn[creepType];
                SpawnCreep(creep.Sample);
                creep.Count--;

                if (creep.Count == 0)
                    creepType++;
                
                _spawnTimer = currentWave.SpawnTime;
            }

            yield return null;
        }
    
        if (++_currentWaveIndex == waves.Length)
            WavesOver = true;

        EventManager.Instance.Invoke("WaveOver", this, EventArgs.Empty);
        WaveIsOn = false;
    }
    
    IEnumerator StartNewBossWave()
    {
        SpawnCreep(bossSample);
        EventManager.Instance.Invoke("BossWaveOver", this, EventArgs.Empty);
        yield return null;
    }

    public void SetTimer(float time)
    {
        var coroutine = WaveTimer(time);
        StartCoroutine(coroutine);
    }

    IEnumerator WaveTimer(float time)
    {
        tableText.enabled = true;

        while (time > 0)
        {
            tableText.text = ((int)time).ToString();
            time -= Time.deltaTime;
            yield return null;
        }

        tableText.enabled = false;
    }
}
