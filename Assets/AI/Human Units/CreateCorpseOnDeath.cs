using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCorpseOnDeath : MonoBehaviour
{
    [SerializeField] GameObject corpsePF;

    public void SpawnCorpse()
    {
        Instantiate(corpsePF, transform.position, Quaternion.identity);
    }
}
