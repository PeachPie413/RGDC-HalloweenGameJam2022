using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// base class all units inherit from, manages health mostly
/// </summary>
public abstract class BaseUnit : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public UnityEvent onDeathEvent;


    public abstract void TakeDamage(int damageAmount);
}
