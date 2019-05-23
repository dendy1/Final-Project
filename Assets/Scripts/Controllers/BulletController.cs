using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameObject _target;
    private float _speed;
    private float _damage;
    
    public float Damage => _damage;
    
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        transform.LookAt(_target.transform);
        
        if (!_target.activeSelf)
            GetComponent<PoolObject>().ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<PoolObject>().ReturnToPool();
    }

    public void SetFields(GameObject target, float speed, float damage)
    {
        _speed = speed;
        _target = target;
        _damage = damage;
    }
}
