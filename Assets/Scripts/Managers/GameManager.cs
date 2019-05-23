using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    
    [Header("Text Fields")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text wavesText;
    [SerializeField] private float textAnimationTime;

    [Header("Game Setup")]
    public GameSettings settings;
    
    private List<SpawnerController> _spawnerControllers;

    private int _currentGold;
    private float _currentHealth;
    private int _currentWave;
    
    private int _creepsCount;
    
    private int _spawnersReady;
    private bool _gameover;

    private string _wavesPrefix = "WAVES REMAINING: ";
    private string _goldPrefix = "GOLD: ";
    private string _healthPrefix = "HEALTH: ";

    public int Gold
    {
        get { return _currentGold; }
        set
        {
            var delta = value - _currentGold;
            
            _currentGold = value;
    
            goldText.DOKill();
            float deltaTime = 0f;
            goldText.DOFade(1f, textAnimationTime).OnUpdate(() =>
            {
                deltaTime += Time.deltaTime;
                var newTime = delta * (1 - deltaTime / textAnimationTime);

                Utils.SetText(_currentGold - delta + (delta - newTime), goldText, _goldPrefix);
            }).OnComplete(() => { Utils.SetText(_currentGold, goldText, _goldPrefix); });
        }
    }

    public float Health
    {
        get => _currentHealth;
        set
        {
            var delta = value - _currentHealth;
            if (value <= 0)
            {
                _currentHealth = 0;
                return;
            }
            
            _currentHealth = value;
            
            healthText.DOKill();
            float deltaTime = 0f;
            
            healthText.DOFade(1f, textAnimationTime).OnUpdate(() =>
            {
                deltaTime += Time.deltaTime;
                var newTime = delta * (1 - deltaTime / textAnimationTime);

                Utils.SetText(_currentHealth - delta + (delta - newTime), healthText, _healthPrefix);
            }).OnComplete(() => { Utils.SetText(_currentHealth, healthText, _healthPrefix); });
        }
    }

    public int CreepsPerWave => settings.CreepsPerWave;

    public GameSettings Settings
    {
        get => settings;
        set => settings = value;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameManager is existing!");
            return;
        }
        
        Instance = this;
    }

    private void Start()
    {
        _currentGold = settings.StartGold;
        _currentHealth = settings.StartHealth;
            
        Utils.SetText(_currentHealth, healthText, _healthPrefix);
        Utils.SetText(_currentGold, goldText, _goldPrefix);
        Utils.SetText(settings.WavesCount, wavesText, _wavesPrefix);
        
        _spawnerControllers = new List<SpawnerController>();
        foreach (var spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            SpawnerController sc = spawner.GetComponent<SpawnerController>();
            _spawnerControllers.Add(sc);
            sc.StartWave();
        }
        
        EventManager.Instance.AddListener("TowerBought", OnTowerBought);
        EventManager.Instance.AddListener("TowerSelled", OnTowerSelled);
        
        EventManager.Instance.AddListener("CreepDied", OnCreepKilled);
        EventManager.Instance.AddListener("CreepAttacked", OnBaseAttacked);
        
        EventManager.Instance.AddListener("WaveOver", OnWaveOver);
        EventManager.Instance.AddListener("CreepAttacked", (sender, args) => _creepsCount++);
    }
    
    private void OnTowerSelled(object sender, EventArgs args)
    {
        Gold += (args as GoldEventArgs).Gold / 2;
    }

    private void OnTowerBought(object sender, EventArgs args)
    {
        Gold -= (args as GoldEventArgs).Gold;
    }

    public void OnBaseAttacked(object sender, EventArgs args)
    {
        Health -= (args as DamageEventArgs).Damage;
        _creepsCount--;
        if (_currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void OnCreepKilled(object sender, EventArgs args)
    {
        Gold += (args as GoldEventArgs).Gold;
        _creepsCount--;
    }

    private void GameOver()
    {
        _gameover = true;
        loseScreen.SetActive(true);
    }

    private void GameWin()
    {
        winScreen.SetActive(true);
    }

    IEnumerator CheckForWin()
    {
        while (_creepsCount > 0)
        {
            yield return null;
        }
        
        GameWin();
    }

    private void OnWaveOver(object sender, EventArgs args)
    {
        if (_gameover)
            return;
        
        _spawnersReady++;

        if (_spawnersReady == _spawnerControllers.Count)
        {
            _spawnersReady = 0;
            
            if (_currentWave < settings.WavesCount)
                StartCoroutine("StartNewWave");
            else
                StartCoroutine("CheckForWin");
        }
    }

    IEnumerator StartNewWave()
    {
        while (_creepsCount > 0)
            yield return null;
        
        _currentWave++;
        Utils.SetText(settings.WavesCount - _currentWave, wavesText, _wavesPrefix);
        
        foreach (var sc in _spawnerControllers)
            sc.SetTimer(settings.WavePeriod);
        
        yield return new WaitForSeconds(settings.WavePeriod);

        foreach (var sc in _spawnerControllers)
            sc.StartWave();
    }
    
    public void RestartLevel()
    {        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
