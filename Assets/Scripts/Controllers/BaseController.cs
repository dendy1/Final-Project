using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creep"))
        {
            var damage = other.GetComponentInParent<CreepStats>().Damage;
            GameManager.Instance.DestroyObject(other.transform.gameObject, true);
            EventManager.Instance.Invoke("BaseAttacked", this, new DamageEventArgs(damage));
        }
    }
}
