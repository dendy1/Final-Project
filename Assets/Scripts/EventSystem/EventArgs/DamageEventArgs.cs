using System;

public class DamageEventArgs : EventArgs
{
    public float Damage { get; set; }

    public DamageEventArgs(float damage)
    {
        Damage = damage;
    }
}
