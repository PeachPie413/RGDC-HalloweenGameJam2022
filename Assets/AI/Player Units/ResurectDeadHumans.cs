using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// any gameobject with this will turn nearby human corpses into undead human units
/// </summary>
public class ResurectDeadHumans : MonoBehaviour
{
    [SerializeField] float resurectRadius;
    [SerializeField] float timeBetweenResurectionAttempts;
    [SerializeField] GameObject undeadHumanPF;
    [SerializeField] LayerMask corpseLayers;
    float timeSinceLastResurection;


    private void Update()
    {
        timeSinceLastResurection += Time.deltaTime;

        //resurect if no longer on cooldown
        if(timeSinceLastResurection > timeBetweenResurectionAttempts)
        {
            timeSinceLastResurection = 0;

            //get all nearby corpses
            Collider2D[] nearbyCorpses = Physics2D.OverlapCircleAll(transform.position, resurectRadius, corpseLayers);
            foreach (Collider2D collider in nearbyCorpses)
            {
                //create undead at corpse spot and destroy corpse
                Instantiate(undeadHumanPF, collider.transform.position, Quaternion.identity);
                Destroy(collider.gameObject);
            }
        }
    }
}
