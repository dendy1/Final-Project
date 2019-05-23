using System;
using UnityEngine;

public class TowerPlatform : MonoBehaviour
{
    [SerializeField] private float onMouseOverScale;
    
    private bool _haveTower;
    private Renderer _renderer;
    private Vector3 _normalScale;
    
    private void Start()
    {
        _normalScale = transform.localScale;
    }

    private void OnMouseOver()
    {
        var tower = BuildManager.Instance.CurrentTower;
        if (!tower)
            return;

        transform.localScale = _normalScale * onMouseOverScale;
    }

    private void OnMouseDown()
    {
        var tower = BuildManager.Instance.CurrentTower;
        
        if (!tower || _haveTower)
            return;
        
        int price = tower.GetComponent<TowerController>().Price;
        if (GameManager.Instance.Gold < price)
            return;

        if (!PoolManager.GetObject(tower.name, transform.position, Quaternion.identity))
        {
            Instantiate(tower, transform.position, Quaternion.identity);
        }

        _haveTower = true;
        
        EventManager.Instance.Invoke("TowerBought", this, new GoldEventArgs(price));
    }

    private void OnMouseExit()
    {
        transform.localScale = _normalScale;
    }
}
