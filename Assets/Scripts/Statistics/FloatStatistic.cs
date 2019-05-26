using System;
using UnityEngine;

[Serializable]
public class FloatStatistic 
{
    [SerializeField] private float baseValue;

    public float Value
    {
        get { return baseValue; }
    }
}
