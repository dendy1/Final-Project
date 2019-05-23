using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public class CreepController : MonoBehaviour
{
    [Header("Creep GameSettings")] [SerializeField]
    private CreepStats creepStats;
    
    [Header("HealthBar Image")]
    [SerializeField] private Image healthBar;

    [Header("Particles")]
    [SerializeField] private GameObject particleEffect;

    private Unit _unit;
    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public void SetTarget(Transform target)
    {
        _unit.Target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        creepStats.TakeDamage(other.GetComponent<BulletController>().Damage);
        healthBar.fillAmount = creepStats.CurrentHealth / creepStats.MaxHealth;
        
        if (creepStats.CurrentHealth <= 0)
        {
            EventManager.Instance.Invoke("CreepDied", this, new GoldEventArgs(creepStats.Gold));
            
            healthBar.fillAmount = 1;
            creepStats.CurrentHealth = creepStats.MaxHealth;
            
            var particle = Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(particle, 2);

            GetComponent<PoolObject>().ReturnToPool();
        }
    }
}
