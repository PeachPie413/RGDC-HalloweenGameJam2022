using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// put this on a unit gameobject to have it take damage over time
/// </summary>
public class DecayEffect : MonoBehaviour
{
    [SerializeField] float timeBetweenDecays;
    [SerializeField] int damagePerDecay;
    BaseUnit unit;
    float timeSinceLastDecay;

    private void Start()
    {
        unit = GetComponent<BaseUnit>();
    }

    private void Update()
    {
        timeSinceLastDecay += Time.deltaTime;

        if(timeSinceLastDecay > timeBetweenDecays)
        {
            timeSinceLastDecay = 0;
            unit.TakeDamage(damagePerDecay);
        }
    }
}
