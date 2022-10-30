using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoreAlienController : BaseUnit
{
    PathCreator pathCreator;
    [SerializeField] float enemyUnitSearchRadius;
    [SerializeField] LayerMask sightLayerMask; //what layers block unit sight?
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] int damageAmount;
    [SerializeField] GameObject projectilePF;
    [SerializeField] float projectileSpeed;
    float timeSinceLastAttack = 0f;

    private void Start()
    {
        pathCreator = GameObject.FindGameObjectWithTag("Middle Tilemap").GetComponent<PathCreator>();
    }


    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        //try to attack
        if(timeSinceLastAttack >= timeBetweenAttacks)
        {
            TryToAttack();
        }
    }



    void TryToAttack()
    {
        //use physics2d to get all nearby colliders
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, enemyUnitSearchRadius);

        //if collider has enemy unit tag, see if there is a sight line to them, if so then attack
        foreach (Collider2D nearbyCollider in nearbyColliders)
        {
            //check if they have enemy unit tag
            if(nearbyCollider.tag == "Enemy Unit")
            {
                //check if can see enemy (nothing between it and the enemy)
                if (!Physics2D.Raycast(transform.position, nearbyCollider.transform.position - transform.position, Vector2.Distance(transform.position, nearbyCollider.transform.position), sightLayerMask))
                {
                    //do attack
                    SpawnProjectile(nearbyCollider.transform.position);

                    //break loop
                    break;
                }
            }
        }

        //reset attack timer
        timeSinceLastAttack = 0f;
    }

    void SpawnProjectile(Vector2 enemyPos)
    {
        //get direction to enemy
        Vector2 dirToEnemy = enemyPos - (Vector2)transform.position;
        dirToEnemy.Normalize();

        //create projectile and set it's velocity
        GameObject projectile = Instantiate(projectilePF, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = dirToEnemy * projectileSpeed;
    }

    public override void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;

        if (currentHP <= 0)
            onDeathEvent.Invoke();
    }
}
