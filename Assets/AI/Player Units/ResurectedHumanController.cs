using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurectedHumanController : BaseUnit
{


    public override void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;

        if(currentHP < 0)
        {
            onDeathEvent.Invoke();
        }
    }
}
