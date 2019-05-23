using UnityEngine;

[CreateAssetMenu]
public class LevelConfig : ScriptableObject 
{
    [System.Serializable]
    public struct PrefabData
    {
        public GameObject Prefab;
        public Vector3 Position;
        public Vector3 Size;
        public Quaternion Rotation;
    }

    public PrefabData[] Objects;
    public GameSettings Settings;
    public GameObject[] TowerPrefabs;
    public PoolManager.PoolPart[] PoolsParts;
}
