using UnityEngine;
using UnityEngine.UI;

public class TowerStats : MonoBehaviour
{    
    [Header("HealthBar Image")]
    [SerializeField] private Image healthBar;

    [Header("Tower Stats")]
    [SerializeField] private IntStatistic maxHealth;
    [SerializeField] private IntStatistic repairCD;
    [SerializeField] private IntStatistic price;
    [SerializeField] private IntStatistic damage;
    [SerializeField] private IntStatistic attackRange;
    [SerializeField] private IntStatistic bulletSpeed;
    [SerializeField] private IntStatistic turnSpeed;
   
    public int Price => price.Value;
    public int Damage => damage.Value;
    public int AttackRange => attackRange.Value;
    public int BulletSpeed => bulletSpeed.Value;
    public int TurnSpeed => turnSpeed.Value;
    public int MaxHealth => maxHealth.Value;
    public int RepairCD => repairCD.Value;
    public bool Broken { get; private set; }
    
    private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth;}
        set
        {
            _currentHealth = value;
            healthBar.fillAmount = CurrentHealth / MaxHealth;
        }
    }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }
    
    public void TakeDamage()
    {
        CurrentHealth -= Damage * 0.05f;

        if (CurrentHealth <= 0)
        {
            Broken = true;
        }
    }

    public void Repair(float value)
    {
        float repairCost = Price * CurrentHealth / MaxHealth;
        if (repairCost > GameManager.Instance.Gold * 0.2f)
            return;

        CurrentHealth += value;
        Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        EventManager.Instance.Invoke("TowerRepaired", this, new GoldEventArgs((int)repairCost));
        Broken = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
