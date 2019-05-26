using System;
using UnityEngine;
using UnityEngine.UI;

public class CreepStats : MonoBehaviour
{
    [Header("HealthBar Image")]
    [SerializeField] private Image healthBar;
    
    [Header("Creep Stats")]
    [SerializeField] private IntStatistic maxHealth;
    [SerializeField] private IntStatistic armor;
    [SerializeField] private IntStatistic damage;
    [SerializeField] private IntStatistic gold;

    private int Armor => armor.Value;
    private int Gold => gold.Value;
    public int Damage => damage.Value;
    public int MaxHealth => maxHealth.Value;

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
    
    public void TakeDamage(float damage)
    {
        damage -= Armor;
        damage = Mathf.Clamp(damage, 0, Int32.MaxValue);

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        EventManager.Instance.Invoke("CreepKilled", this, new GoldEventArgs(Gold));
        healthBar.fillAmount = 1;
    }
}
