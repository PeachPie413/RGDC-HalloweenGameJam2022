using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplodeOnCollision : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField] LayerMask layersToTouch, layersToDamage, layersToShieldBlast;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(layersToTouch == (layersToTouch | (1 << collision.gameObject.layer)))
        {
            //explode
            Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position, range);

            foreach (Collider2D hitCollider in collidersHit)
            {
                float distBetween = Vector2.Distance(hitCollider.transform.position, transform.position);

                //if nothing is stopping blast
                if (!Physics2D.Raycast(transform.position, hitCollider.transform.position - transform.position, distBetween, layersToShieldBlast))
                {
                    //try to damage collider
                    BaseUnit baseUnit;
                    if(hitCollider.TryGetComponent(out baseUnit))
                    {
                        baseUnit.TakeDamage(damage);
                    }
                }
            }
        }
    }
}
