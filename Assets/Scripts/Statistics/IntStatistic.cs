using System;
using UnityEngine;

[Serializable]
public class IntStatistic
{
    [SerializeField] private int baseValue;

    public int Value
    {
        get { return baseValue; }
    }
}
