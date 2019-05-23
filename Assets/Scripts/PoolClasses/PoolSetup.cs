using UnityEngine;

public class PoolSetup : MonoBehaviour
{
    [SerializeField] private PoolManager.PoolPart[] pools;
   
    void OnValidate() 
    {
        for (int i = 0; i < pools.Length; i++) 
        {
            pools[i].name = pools[i].prefab.name;
        }
    }
    
    void Awake() 
    {
        Initialize();
    }

    void Initialize() 
    {
        PoolManager.Initialize(pools);
    }

    public PoolManager.PoolPart[] Pools
    {
        get { return pools; }
        set { pools = value; }
    }
}
