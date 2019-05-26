using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public class CreepController : MonoBehaviour
{
    CreepStats _stats;
    
    [Header("Particles")]
    [SerializeField] private GameObject particleEffect;

    private Unit _unit;
    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _stats = GetComponent<CreepStats>();
    }

    public void SetTarget(Vector3 target)
    {
        _unit.SetTarget(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        _stats.TakeDamage(other.GetComponent<BulletController>().Damage);

        if (_stats.CurrentHealth <= 0)
        {          
            _stats.CurrentHealth = _stats.MaxHealth;
            
            var particle = Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(particle, 2);

            GameManager.Instance.DestroyObject(gameObject);
        }
    }
}
