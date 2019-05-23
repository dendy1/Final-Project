using System;
using UnityEngine;

public static class PoolManager
{
    private static PoolPart[] _pools;
    private static GameObject _objectsParent;
    
    [Serializable]
    public struct PoolPart 
    {
        public string name;
        public PoolObject prefab;
        public int count;
        public ObjectPooling ferula;
    }
    
    public static void Initialize(PoolPart[] newPools) 
    {
        _pools = newPools;
        _objectsParent = new GameObject();
        _objectsParent.name = "Pool";
        
        for (int i=0; i<_pools.Length; i++) 
        {
            if(_pools[i].prefab != null) 
            {  
                _pools[i].ferula = new ObjectPooling();
                _pools[i].ferula.Initialize(_pools[i].count, _pools[i].prefab, _objectsParent.transform);
            }
        }
    }
    
    public static GameObject GetObject(string name, Vector3 position, Quaternion rotation) 
    {
        GameObject result = null;
        
        if (_pools != null) 
        {
            for (int i = 0; i < _pools.Length; i++) 
            {
                if (string.Compare(_pools[i].name, name) == 0) 
                {
                    result = _pools[i].ferula.GetObject ().gameObject;
                    result.transform.position = position;
                    result.transform.rotation = rotation; 
                    result.SetActive (true);
                    return result;
                }
            }
        } 
        
        return result;
    }
}
