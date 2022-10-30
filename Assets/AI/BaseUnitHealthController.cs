using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// a class that inherits from base unit to do all of it's health stuff. Has hook for damage taken and destroys self on death
/// </summary>
public class BaseUnitHealthController : BaseUnit
{
    public UnityEvent onDamaged;

    public override void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;

        if(currentHP <= 0)
        {
            onDeathEvent.Invoke();
            Destroy(gameObject);
        }
        else
        {
            onDamaged.Invoke();
        }
    }
}
