using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemyUnit : BaseUnit
{
    [SerializeField] GameObject corpsePF;

    public override void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;

        if (currentHP <= 0)
        {
            //invoke on death event
            onDeathEvent.Invoke();

            //destroy self and create corpse
            Instantiate(corpsePF, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }



}
