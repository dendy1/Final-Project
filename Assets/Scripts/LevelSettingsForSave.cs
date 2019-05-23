using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettingsForSave : MonoBehaviour
{
    [Header("Game Setup")]
    public GameSettings GameSettings;
    
    [Header("Pool Setup")]
    public PoolManager.PoolPart[] Pools;

    [Header("Tower Setup")] 
    public GameObject[] TowerPrefabs;
}
