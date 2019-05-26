using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Header("Shop settings")]
    public GameObject shopContainer;
    public GameObject buttonPrefab;
    
    private Animator _animator;
    private GameObject[] _towers;

    void Start()
    {
        _animator = shopContainer.GetComponent<Animator>();
        _towers = ShopManager.Instance.Towers;
        
        for (int i = 0; i < _towers.Length; i++)
        {
            var button = Instantiate(buttonPrefab);
            button.GetComponent<RectTransform>().SetParent(shopContainer.transform);
            button.transform.GetChild(0).GetComponent<Text>().text = _towers[i].name;
            button.transform.GetChild(1).GetComponent<Text>().text = _towers[i].GetComponent<TowerStats>().Price.ToString();
            var towerPrefab = _towers[i];
            button.GetComponent<Button>().onClick.AddListener(() => BuyTower(towerPrefab));
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            GameManager.Instance.ShopMenuOpened = true;
        
        if (Input.GetKeyUp(KeyCode.Tab))
            GameManager.Instance.ShopMenuOpened = false;
        
        
        if (GameManager.Instance.ShopMenuOpened)
            OpenShop();
        else
            CloseShop();
    }

    private void OpenShop()
    {
        if (!_animator.GetBool("Open"))
            _animator.SetBool("Open", true);
    }

    private void CloseShop()
    {
        _animator.SetBool("Open", false);
    }
    
    public void BuyTower(GameObject towerPrefab)
    {
        BuildManager.Instance.CurrentTower = towerPrefab;
    }
}
