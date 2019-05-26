using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Transform _target;
    private float _speed;
    
    public int Damage { get; set; }
    
    private void Update()
    {
        if (!_target || !_target.gameObject.activeSelf || !transform)
        {
            GameManager.Instance.DestroyObject(gameObject);
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        transform.LookAt(_target.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.DestroyObject(gameObject);
    }

    public void SetFields(Transform target, float speed, int damage)
    {
        _speed = speed;
        _target = target;
        Damage = damage;
    }
}
