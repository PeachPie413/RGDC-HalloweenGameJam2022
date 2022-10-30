using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingProjectileController : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //try to hurt hit object
        BaseUnit baseUnit;
        if (collision.gameObject.TryGetComponent<BaseUnit>(out baseUnit))
        {
            baseUnit.TakeDamage(damage);
        }

        //destroy self
        Destroy(gameObject);
    }
}
