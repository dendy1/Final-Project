using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class TowerController : MonoBehaviour
{       
    [Header("Shooting settings")]
    public GameObject bulletSample;
    public float shootDelay;
    public Transform rotationPart;
    public Transform shootingPivots;
    
    [Header("Tower settings")]
    [SerializeField] private string towerName;

    private TowerStats _stats;

    private bool _shooting;
    private bool _broken;
    private GameObject _target;

    private void Awake()
    {
        if (string.IsNullOrEmpty(towerName))
        {
            towerName = gameObject.name;
        }
        
        _stats = GetComponent<TowerStats>();
    }

    private void Update()
    {
        if (!_target || _stats.Broken)
            return;      
        
        Vector3 direction = _target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 rotation = Quaternion.Lerp(rotationPart.rotation, lookRotation, Time.deltaTime * _stats.TurnSpeed).eulerAngles;
        rotationPart.rotation = Quaternion.Euler(0, rotation.y, 0);
        
        if (_click && Time.time > _clickTime + interval) 
        {
            _click = false;
        }
        
        if (!_shooting)
            StartCoroutine("Shoot");
    }

    private void FixedUpdate()
    {
        if (_stats.Broken)
            return;
        
        FindClosestTarget();   
    }

    private int _gunIndex = 0;
    private int CurrentGunIndex => Mathf.Clamp(_gunIndex++, 0, shootingPivots.childCount - 1);
    private Transform CurrentGun => shootingPivots.GetChild(CurrentGunIndex);

    private void FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Creep");

        float minDistance = Mathf.Infinity;
        GameObject tempTarget = null;
        
        foreach (var target in targets)
        {
            float currentDistance = Vector3.Distance(transform.position, target.transform.position);
            if (currentDistance < minDistance)
            {
                tempTarget = target;
                minDistance = currentDistance;
            }
        }

        if (tempTarget && minDistance <= _stats.AttackRange)
            _target = tempTarget;
        else
            _target = null;
    }

    IEnumerator Shoot()
    {
        if (!_target)
            yield return null;
        
        _shooting = true;
        var cg = CurrentGun;
        var pool = PoolManager.GetObject(bulletSample.name, cg.position, Quaternion.identity);
        if (!pool)
        {
            pool = Instantiate(bulletSample, cg.position, Quaternion.identity);
        }
        pool.GetComponent<BulletController>().SetFields(_target.transform, _stats.BulletSpeed, _stats.Damage);
        
        _stats.TakeDamage();

        yield return new WaitForSeconds(shootDelay);
        _shooting = false;
    }

    readonly float interval = 0.4f;
    private bool _click;
    private float _clickTime;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (_click && Time.time <= _clickTime + interval)
            {
                OnDoubleClick();
                _click = false;
            }
            else 
            {
                _click = true;
                _clickTime = Time.time;
            }
        }
    }

    private void OnDoubleClick()
    {
        GameManager.Instance.DestroyObject(gameObject);
        EventManager.Instance.Invoke("TowerSold", this, new GoldEventArgs(_stats.Price));     
    }
}
