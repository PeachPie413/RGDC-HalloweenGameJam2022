using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// put this on a gameobejct to have it go the the nearest gameobject that is the given layer
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MoveToNearestTarget : MonoBehaviour
{
    PathCreator pathCreator;
    [SerializeField] float timeBetweenPathGeneration;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float targetSearchDist;
    [SerializeField] float moveSpeed;
    float timeSinceLastPathGeneration;
    List<Vector2Int> path = new List<Vector2Int>();
    Rigidbody2D rb;


    private void Start()
    {
        pathCreator = PathCreator.instance;
        rb = GetComponent<Rigidbody2D>();
        GetPath();
    }


    private void Update()
    {
        timeSinceLastPathGeneration += Time.deltaTime;

        if (timeSinceLastPathGeneration > timeBetweenPathGeneration)
        {
            GetPath();
        }

        //try to remove node from path if it reached it
        if (path.Count > 0 && Vector2.Distance(path[0] + new Vector2(0.5f, 0.5f), transform.position) < 0.5f)
        {
            path.RemoveAt(0);
        }

        FollowPath();
    }

    void GetPath()
    {
        timeSinceLastPathGeneration = 0;

        Vector2 tagretPos = GetPosOfNearestEnemy();

        //if target pos is pos infinity them nea sno tagret in range
        if (tagretPos == Vector2.positiveInfinity)
        {
            path = new List<Vector2Int>();
        }
        else
            path = pathCreator.GetPath(transform.position, tagretPos);
    }

    void FollowPath()
    {
        //if path has no items, do nothing
        if(path.Count == 0)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            //get next node
            Vector2 nextNode = path[0];
            nextNode += new Vector2(0.5f, 0.5f);

            Vector2 moveDir = nextNode - (Vector2)transform.position;
            moveDir.Normalize();

            rb.velocity = moveDir * moveSpeed;
        }
    }


    Vector2 GetPosOfNearestEnemy()
    {
        Collider2D[] targetCollidersInRange = Physics2D.OverlapCircleAll(transform.position, targetSearchDist, layerMask);

        float smallestDist = 1000f;
        Vector2 closestTagret = Vector2.positiveInfinity;
        foreach (Collider2D targetCollider in targetCollidersInRange)
        {
            float dist = Vector2.Distance(targetCollider.transform.position, transform.position);

            if(dist < smallestDist)
            {
                smallestDist = dist;
                closestTagret = targetCollider.transform.position;
            }
        }

        return closestTagret;
    }


    private void OnDrawGizmos()
    {
        if(path.Count > 1)
        {
            Gizmos.color = Color.yellow;

            for (int i = path.Count - 1; i >= 1; i--)
            {
                Gizmos.DrawLine(new Vector3(path[i].x + 0.5f, path[i].y + 0.5f), new Vector3(path[i - 1].x + 0.5f, path[i - 1].y + 0.5f));
            }
        }
    }
}
