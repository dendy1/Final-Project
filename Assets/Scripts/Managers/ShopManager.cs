using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; set; }
    
    public GameObject[] Towers;

    private void Awake()
    {      
        if (Instance != null)
        {
            Debug.LogError("ShopManager is existing!");
            return;
        }

        Instance = this;
    }
}
