using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }
    public GameObject CurrentTower { get; set; }
    public string TowerName { get; set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("BuildManager is existing!");
            return;
        }
        
        Instance = this;
    }
}
