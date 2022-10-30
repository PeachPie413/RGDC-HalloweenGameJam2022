using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] GameObject projectilePF;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float range;
    [SerializeField] float projectileSpeed;
    float timeSinceLastShot;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] LayerMask sightBlockLayers;
    public UnityEvent onShotFired;


    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if(timeSinceLastShot > timeBetweenShots)
        {
            timeSinceLastShot = 0;

            //use physics2d to get all nearby colliders
            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, range, targetLayers);

            //if collider has enemy unit tag, see if there is a sight line to them, if so then attack
            Vector2 closeestTargetPos = Vector2.zero;
            float closestTargetDist = float.PositiveInfinity;
            foreach (Collider2D nearbyCollider in nearbyColliders)
            {
                //check if can see enemy (nothing between it and the enemy)
                if (!Physics2D.Raycast(transform.position, nearbyCollider.transform.position - transform.position, Vector2.Distance(transform.position, nearbyCollider.transform.position), sightBlockLayers))
                {
                    float distance = Vector2.Distance(transform.position, nearbyCollider.transform.position);
                    if(distance < closestTargetDist)
                    {
                        closestTargetDist = distance;
                        closeestTargetPos = nearbyCollider.transform.position;
                    }
                }
            }

            if(closestTargetDist != float.PositiveInfinity)
            {
                SpawnProjectile(closeestTargetPos);
            }
        }
    }


    void SpawnProjectile(Vector2 targetPos)
    {
        //get direction to enemy
        Vector2 dirToEnemy = targetPos - (Vector2)transform.position;
        dirToEnemy.Normalize();

        //create projectile and set it's velocity
        GameObject projectile = Instantiate(projectilePF, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = dirToEnemy * projectileSpeed;
    }
}
