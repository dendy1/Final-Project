using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepStats : MonoBehaviour
{
    public int MaxHealth = 100;
    public float CurrentHealth { get; set; }

    [SerializeField] private Statistic armor;
    [SerializeField] private Statistic damage;
    [SerializeField] private Statistic gold;

    public int Armor => armor.Value;
    public int Damage => damage.Value;
    public int Gold => gold.Value;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        damage -= armor.Value;
        damage = Mathf.Clamp(damage, 0, Int32.MaxValue);

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        EventManager.Instance.Invoke("CreepDied", this, new GoldEventArgs(Gold));
        CurrentHealth = MaxHealth;
    }
}
