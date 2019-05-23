using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class LevelEditor : MonoBehaviour 
{
    public string levelName;
    public LevelConfig Level;

    private LevelSettingsForSave _currentSettings;

    private void OnValidate()
    {
        levelName = Level.name;
        _currentSettings.Pools = Level.PoolsParts;
        _currentSettings.GameSettings = Level.Settings;
        _currentSettings.TowerPrefabs = Level.TowerPrefabs;
    }

    public void Save()
    {
        var levelSettings = FindObjectOfType(typeof(LevelSettingsForSave)) as LevelSettingsForSave;

        if (!levelSettings)
        {
            Debug.LogError("Please, use one active LevelSettingsForSave script on a GameObject in your scene.");
            return;
        }

        Level = ScriptableObject.CreateInstance<LevelConfig>();
        AssetDatabase.CreateAsset(Level, "Assets/Resources/Levels/" + levelName + ".asset");
        EditorUtility.SetDirty(Level);

        var objectsList = Utils.FindGameObjectsWithTags("WalkableArea", "Obstacle", "Platform", "Spawner", "Base");
        var towerPrefabs = levelSettings.TowerPrefabs;
        var pools = levelSettings.Pools;

        Level.Objects = new LevelConfig.PrefabData[objectsList.Count];
        for (int i = 0; i < objectsList.Count; ++i)
        {
            var obj = objectsList[i];
            
            Level.Objects[i] = new LevelConfig.PrefabData
            {
                Prefab = (GameObject)PrefabUtility.GetPrefabParent(obj.gameObject), 
                Position = obj.transform.position,
                Size = obj.transform.localScale,
                Rotation =  obj.transform.rotation
            };
            Level.Settings = levelSettings.GameSettings;
            Level.TowerPrefabs = towerPrefabs;
            Level.PoolsParts = pools;
        }
    }
}