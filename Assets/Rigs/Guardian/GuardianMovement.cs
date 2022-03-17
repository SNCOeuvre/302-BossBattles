using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//makes it required to have the animator thing
[RequireComponent(typeof(Animator))]
public class GuardianMovement : MonoBehaviour
{
    public float walkSpeed = 5;
    private Animator animator;

    void Start()
    {
        //calls a reference to the animator
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        float v = Input.GetAxis("Vertical");


        //1 meter times the forward/backward axis over delta time times the walk speed;
        transform.position += transform.forward * v * Time.deltaTime * walkSpeed;


        animator.SetFloat("Walk Speed", Mathf.Abs(v * walkSpeed));

    }
}
