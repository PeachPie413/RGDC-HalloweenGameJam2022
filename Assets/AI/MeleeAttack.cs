using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] float timeBetweenAttacks;
    float timeSinceLastAttack;
    [SerializeField] int damagePerAttack;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask targetLayer;
    UnityEvent onAttackEvent;
    public Vector2 attackTargetPos;


    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;


        if(timeSinceLastAttack > timeBetweenAttacks)
        {
            timeSinceLastAttack = 0f;

            Collider2D target = Physics2D.OverlapCircle(transform.position, attackRange, targetLayer);
            BaseUnit targetUnit;
            if(target != null && target.TryGetComponent<BaseUnit>(out targetUnit))
            {
                targetUnit.TakeDamage(damagePerAttack);
                attackTargetPos = targetUnit.transform.position;
            }
            else
            {
                attackTargetPos = Vector2.zero;
            }
        }
    }
}
