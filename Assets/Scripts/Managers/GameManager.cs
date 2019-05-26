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

    [Header("Screens")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [Header("Text Fields")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text wavesText;
    [SerializeField] private float textAnimationTime;

    [Header("Base Setup")] [SerializeField]
    private GameObject baseObject;
    
    [Header("Game Setup")]
    public GameSettings settings;
    
    private List<SpawnerController> _spawnerControllers;

    private int _currentGold;
    private float _currentHealth;
    private int _creepsCount;

    private float _score;
    
    private int _spawnersReady;
    private bool _gameOver;
    private int _wavesCount;
    private bool _bossFight;

    private string _wavesPrefix = "NEW WAVE IN: ";
    private string _goldPrefix = "GOLD: ";
    private string _healthPrefix = "HEALTH: ";

    private bool WavesOver
    {
        get
        {
            foreach (var sc in _spawnerControllers)
            {
                if (!sc.WavesOver)
                    return false;
            }

            return true;
        }
    }

    private bool OnGoingWaves
    {
        get
        {
            foreach (var sc in _spawnerControllers)
            {
                if (sc.WaveIsOn)
                    return true;
            }

            return false;
        }
    }

    public int Gold
    {
        get { return _currentGold; }
        private set
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

    private float Health
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
    
    public bool InputMenuOpened { get; set; }
    public bool ShopMenuOpened { get; set; }

    public bool FinalWave { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameManager is existing!");
            return;
        }
        
        Instance = this;
        
        EventManager.Instance.AddListener("TowerBought", (sender, args) =>
        {
            Gold -= (args as GoldEventArgs).Gold;
            _score += (args as GoldEventArgs).Gold;
        });
        EventManager.Instance.AddListener("TowerSold", (sender, args) =>
        {
            Gold += (int)((args as GoldEventArgs).Gold * 0.5f);
            _score += (args as GoldEventArgs).Gold * 0.1f;
        });

        EventManager.Instance.AddListener("BaseAttacked", OnBaseAttacked);
        EventManager.Instance.AddListener("WaveOver", OnWaveOver);
        EventManager.Instance.AddListener("BossWaveOver", OnBossWaveOver);
        
        EventManager.Instance.AddListener("CreepKilled", OnCreepKilled);
        EventManager.Instance.AddListener("CreepSpawned", (sender, args) => _creepsCount++);
        
        EventManager.Instance.AddListener("TowerRepaired", (sender, args) =>
        {
            Gold -= (args as GoldEventArgs).Gold;
            _score += (args as GoldEventArgs).Gold * 0.1f;
        });
    }

    private void Start()
    {
        _currentGold = settings.StartGold;
        _currentHealth = settings.StartHealth;
            
        Utils.SetText(_currentHealth, healthText, _healthPrefix);
        Utils.SetText(_currentGold, goldText, _goldPrefix);
        
        _spawnerControllers = new List<SpawnerController>();
        foreach (var spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            SpawnerController sc = spawner.GetComponent<SpawnerController>();
            _spawnerControllers.Add(sc);
            sc.StartWave();
        }
    }

    public void OnBaseAttacked(object sender, EventArgs args)
    {
        Health -= (args as DamageEventArgs).Damage;
        _score += (args as DamageEventArgs).Damage * 0.3f;
        _creepsCount--;
        if (_currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void OnCreepKilled(object sender, EventArgs args)
    {
        Gold += (args as GoldEventArgs).Gold;
        _score += (args as GoldEventArgs).Gold * 0.8f;
        _creepsCount--;
    }

    private void GameOver()
    {
        _gameOver = true;
        loseScreen.SetActive(true);
    }

    private void GameWin()
    {
        winScreen.SetActive(true);
        winScreen.transform.GetChild(1).GetComponent<Text>().text = "Ваш счёт: " + (int)_score;
    }

    IEnumerator CheckForWin()
    {
        while (_creepsCount > 0)
        {
            yield return null;
        }
        
        if (!_gameOver)
            GameWin();
    }

    private void OnWaveOver(object sender, EventArgs args)
    {
        if (_gameOver)
            return;
        
        _spawnersReady++;

        if (_spawnersReady == _spawnerControllers.Count)
        {
            _spawnersReady = 0;
            StartCoroutine("StartWave");
        }
    }

    private void OnBossWaveOver(object sender, EventArgs args)
    {
        if (_gameOver)
            return;
        
        _spawnersReady++;

        if (_spawnersReady == _spawnerControllers.Count)
        {
            _spawnersReady = 0;
            StartCoroutine("CheckForWin");
        }
    }

    IEnumerator StartWave()
    {
        StartCoroutine("BossFight");

        while (_bossFight)
            yield return null;
        
        if (!WavesOver)
            StartCoroutine("StartNewWave");
        else
            StartCoroutine("StartFinalWave");
            
    }

    IEnumerator StartNewWave()
    {
        while (_creepsCount > 0 || OnGoingWaves)
            yield return null;

        StartCoroutine(SetTime(settings.WaveBreak, _wavesPrefix));
        yield return new WaitForSeconds(settings.WaveBreak);
        
        foreach (var sc in _spawnerControllers)
            sc.StartWave();
    }

    IEnumerator BossFight()
    {
        yield return null;
    }
    
    IEnumerator StartFinalWave()
    {
        while (_creepsCount > 0 || OnGoingWaves)
            yield return null;

        StartCoroutine(SetTime(settings.WaveBreak, "BOSS COMING IN: "));
        yield return new WaitForSeconds(settings.WaveBreak);
        
        foreach (var sc in _spawnerControllers)
            sc.StartFinalWave();

        yield return null;
    }

    private float _time;
    IEnumerator SetTime(float waveBreak, string prefix)
    {
        wavesText.gameObject.SetActive(true);
        _time = waveBreak;
        
        while (_time > 0)
        {
            _time -= Time.deltaTime;
            Utils.SetText(string.Format("{0:f}", _time), wavesText, prefix);
            yield return null;
        }
        
        wavesText.gameObject.SetActive(false);
    }
    
    public void RestartLevel()
    {        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void DestroyObject(GameObject obj, bool parent = false)
    {
        if (parent)
            obj = obj.transform.root.gameObject;

        var poolTest = obj.GetComponent<PoolObject>();
        
        if (poolTest)
            poolTest.ReturnToPool();
        else 
            Destroy(obj);
    }

    public GameObject SpawnObject(GameObject obj, Vector3 position, Quaternion rotation)
    {
        var spawn = PoolManager.GetObject(obj.name, position, rotation);
        if (!spawn)
            spawn = Instantiate(obj, position, rotation);

        return spawn;
    }

    public void OpenCloseShop()
    {
        ShopMenuOpened = !ShopMenuOpened;
    }
}
