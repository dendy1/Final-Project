using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private FloatStatistic repair;

    public float Repair => repair.Value;
}
