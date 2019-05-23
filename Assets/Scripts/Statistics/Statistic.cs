using System;
using UnityEngine;

[Serializable]
public class Statistic
{
    [SerializeField] private int baseValue;

    public int Value
    {
        get { return baseValue; }
    }
}
