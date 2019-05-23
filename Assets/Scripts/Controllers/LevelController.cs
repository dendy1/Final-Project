using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Level File")]
    [SerializeField] private LevelConfig startLevel;

    [Header("Containers")]
    public Transform walkableAreasContainer;
    public Transform obstaclesContainer;
    public Transform buildingsContainer;
    public Transform platformsContainer;

    private LevelConfig _level;
    private LevelConfig _prevStartLevel;

    private void Start()
    {
        if (startLevel)
        {
            ExecuteLevel(startLevel);
            _prevStartLevel = startLevel;
        }
    }

    private void ExecuteLevel(LevelConfig level)
    {
        _level = level;
        InitLevel();
    }

    private void InitLevel()
    {
        foreach (var obj in _level.Objects)
        {
            var poolObj = PoolManager.GetObject(obj.Prefab.name, obj.Position, obj.Rotation);
            if (!poolObj)
            {
                var go = Instantiate(obj.Prefab, obj.Position, obj.Rotation);
                go.transform.localScale = obj.Size;
                go.transform.SetParent(Parent(go.tag));
            }
            else 
                poolObj.transform.SetParent(Parent(poolObj.tag));
        }
    }

    private Transform Parent(string childTag)
    {
        switch (childTag)
        {
            case "WalkableArea":
                return walkableAreasContainer;
            
            case "Obstacle":
                return obstaclesContainer;
            
            case "Platform":
                return platformsContainer;
            
            case "Spawner": case "Base":
                return buildingsContainer;
            
            default:
                throw new Exception();
        }
    }

    private void OnValidate()
    {
        var gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        gameManager.settings = startLevel.Settings;

        var poolSetup = FindObjectOfType(typeof(PoolSetup)) as PoolSetup;
        poolSetup.Pools = startLevel.PoolsParts;

        var shopManager = FindObjectOfType(typeof(ShopManager)) as ShopManager;
        shopManager.Towers = startLevel.TowerPrefabs;
    }
}
