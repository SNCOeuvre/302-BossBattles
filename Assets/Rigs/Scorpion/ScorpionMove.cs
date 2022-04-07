using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ScorpionMove : MonoBehaviour
{

    enum Mode
    {
        Idle,
        Walk,
        Jump
        //in air, fall
    }
    public ScorpionStickyFeet[] feet;

    public float speed = 2;
    public Transform groundRing;
    private CharacterController pawn;
    private Mode mode = Mode.Idle;
    private Vector3 input;
    private Camera cam;
    private Quaternion targetRotation;
    private Vector3 groundRingTarget;
    /// <summary>
    /// The current vertical velocity in meters/second
    /// </summary>
    public float velocityY = 0;
    public float gravity = 9.8f;
    //not a force, doesn't get multiplied by deltatime
    public float jumpImpulse = 9.8f;

    void Start()
    {
        pawn = GetComponent<CharacterController>();
        cam = Camera.main;
    }


    void LateUpdate()
    {
        GetPlayerInputRelativeToCamera();

       

        


        float threshold = .1f;
        //set movement mode based on movement input using ternart statement
        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle;

        
        if (pawn.isGrounded)
        {
            //checks on the frame it is pressed
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = -jumpImpulse;
            }
        }

        velocityY += gravity * Time.deltaTime;
        //applies delta movement times public speed, input, and gravity
        pawn.Move((input * speed + Vector3.down * velocityY) * Time.deltaTime);
        if (pawn.isGrounded)
        {
            velocityY = 0;
        }
        else
        {
            mode = Mode.Jump;
        }

        //animate feet?
        Animate();

    }

    void Animate()
    {
        groundRingTarget = transform.InverseTransformDirection(input) + new Vector3(0, -.3f, 0);
        groundRing.localPosition = AnimMath.Ease(groundRing.localPosition, groundRingTarget, .001f);

        if (mode == Mode.Walk) targetRotation = Quaternion.LookRotation(input, Vector3.up);

        transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f);

        switch (mode)
        {
            case Mode.Idle:
                AnimateIdle();
                break;
            case Mode.Walk:
                AnimateWalk();
                break;
            case Mode.Jump:
                AnimateJump();
                break;
        }
    }

    void AnimateJump()
    {

    }


    //makes a new green thing that can become a data type of
    delegate void MoveFoot(float time, FootRaycast foot);

    void AnimateIdle()
    {



    }

    void AnimateWalk()
    {
        
    }

    private void GetPlayerInputRelativeToCamera()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //cam without the y
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        //cam without the
        Vector3 camSide = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camSide * h;

        if (input.sqrMagnitude > 1) input.Normalize();
    }

}
