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
        _normalScale = transform.GetChild(0).localScale;
    }

    private void OnMouseOver()
    {
        var tower = BuildManager.Instance.CurrentTower;

        if (!tower || _haveTower)
            return;
        
        transform.GetChild(0).localScale = _normalScale * onMouseOverScale;
    }

    private void OnMouseDown()
    {
        var tower = BuildManager.Instance.CurrentTower;
        if (!tower || _haveTower)
            return;
        
        EventManager.Instance.AddListener("TypoSucceed", OnTypoSucceed);
        WordsManager.Instance.ShowInputPanel();
    }

    private void OnMouseExit()
    {
        transform.GetChild(0).localScale = _normalScale;
    }

    private void OnTypoSucceed(object sender, EventArgs args)
    {
        var tower = BuildManager.Instance.CurrentTower;
        
        if (!tower || _haveTower)
            return;

        var time = (args as TypoEventArgs).Time;
        
        int price = (int)(tower.GetComponent<TowerController>().Price * (time / 6));

        if (GameManager.Instance.Gold < price)
            return;

        if (!PoolManager.GetObject(tower.name, transform.position, Quaternion.identity))
        {
            Instantiate(tower, transform.position, Quaternion.identity);
        }

        _haveTower = true;
        BuildManager.Instance.CurrentTower = null;
        EventManager.Instance.Invoke("TowerBought", this, new GoldEventArgs(price));
        EventManager.Instance.RemoveListener("TypoSucceed", OnTypoSucceed);
    }
}
