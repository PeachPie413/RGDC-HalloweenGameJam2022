using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    //Movement variables
    public float moveSpeed = 4f;

    //Animation variables
    Animato thisAnim;
    float lastX, lastY;

    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    {
        Vector3 rightMovement = Vector3.right * moveSpeed * time.dealtaTime * Input.GetAxis("Horizontal"); //might want to replace input
        Vector3 upMovement = Vector3.up * moveSpeed * time.dealtaTime * Input.GetAxis("Vertical");
    
         Vctor3 heading = Vector3.Normalize(rightMovement + upMovement);
         transform.position += rightMovement;
         transform.position += upMovement;
         UpdateAnimation(heading);
    }

    void UpdateAnimation(Vector3 dir)
    {
        if(dir.x == 0f && dir.y ==0f)
        {
            //execute idle
            thisAnim.SetFloat("lastDirX", lastX);
            thisAnim.SetFloat("lastDirY", lastY);
            thisAnim.SetBool("Moving", false);
        }else
        {
            lastX = dir.x;
            lastY = dir.y;
            thisAnim.SetBool("Moving", true);
        }

        thisAnim.SetFloat("lastDirX", lastX);
        thisAnim.SetFloat("lastDirY", lastY);
        thisAnim.SetBool("Moving", true);
    }
}
