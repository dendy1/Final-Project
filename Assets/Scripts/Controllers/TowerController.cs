using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class TowerController : MonoBehaviour
{
    [Header("Shooting settings")]
    public GameObject bullet;
    public float shootDelay;
    public Transform rotationPart;
    public Transform shootingPivots;
    
    [Header("Tower settings")]
    [SerializeField] private string name;

    [Range(0,10)]
    [SerializeField] private float range;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float damage;
    [SerializeField] private int price;

    private bool _isShooting;    
    private GameObject _target;

    public int Price => price;

    private void Start()
    {
        if (string.IsNullOrEmpty(name))
        {
            name = gameObject.name;
        }
    }

    private void Update()
    {
        if (!_target)
            return;      
        
        Vector3 direction = _target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 rotation = Quaternion.Lerp(rotationPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        rotationPart.rotation = Quaternion.Euler(0, rotation.y, 0);
        
        if (_click && Time.time > _clickTime + interval) 
        {
            _click = false;
        }
        
        if (!_isShooting)
            StartCoroutine("Shoot");
    }

    private void FixedUpdate()
    {
        FindClosestTarget();   
    }

    private int _gunIndex;
    private int CurrentGunIndex
    {
        get
        {
            var temp = _gunIndex++;
            
            if (_gunIndex >= shootingPivots.childCount)
            {
                _gunIndex = 0;
            }

            return temp;
        }
    }

    private Transform CurrentGun
    {
        get
        {
            return shootingPivots.GetChild(CurrentGunIndex);   
        }
    }

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

        if (tempTarget && minDistance <= range)
        {
            _target = tempTarget;
        }
        else
        {
            _target = null;
        }
    }

    IEnumerator Shoot()
    {
        if (!_target)
            yield return null;
        
        _isShooting = true;
        var b = PoolManager.GetObject(bullet.name, CurrentGun.position, Quaternion.identity);
        b.GetComponent<BulletController>().SetFields(_target, bulletSpeed, damage);
        yield return new WaitForSeconds(shootDelay);
        _isShooting = false;
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
        EventManager.Instance.Invoke("TowerSelled", this, new GoldEventArgs(price));
        
        var po = GetComponent<PoolObject>();
        if (!po)
            Destroy(gameObject);
        else
            po.ReturnToPool();
        
    }

    private void OnDrawGizmos()
    {
        rotationPart = transform.GetChild(0);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
