using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//forces unity to have a catch for runtime errors
[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour
{

    enum Mode
    {
        Idle,
        Walk,
        Jump
            //in air, fall
    }

    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float walkFootSpeed = 2;
    public float footSeperateAmount = 2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    private CharacterController pawn;
    private Mode mode = Mode.Idle;
    private Vector3 input;
    private float walkTime;
    private Camera cam;
    private Quaternion targetRotation;
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
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //cam without the y
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        //cam without the
        Vector3 camSide = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camSide * h;

        if (input.sqrMagnitude > 1) input.Normalize();


        float threshold = .1f;
        //set movement mode based on movement input using ternart statement
        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle;
                
        if(mode == Mode.Walk) targetRotation = Quaternion.LookRotation(input, Vector3.up);

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
        //todo:

        //lift legs
        //lift hands
        //air drift
        //spikes move
        //use vertical velocity
    }


    //makes a new green thing that can become a data type of
    delegate void MoveFoot(float time, FootRaycast foot);

    void AnimateIdle()
    {
        footLeft.SetPositionHome();
        footRight.SetPositionHome();
        
        
    }

    void AnimateWalk()
    {
        //goal: Move y axis for up and z axis for forward
        //don't repeat yourself!

        //anon function
        MoveFoot moveFoot = (t, foot) =>
        {
            //floats for up and down and forward and back
            float y = Mathf.Cos(t) * walkSpreadY; //vertical movement
            float lateral = Mathf.Sin(t) * walkSpreadZ; //[-1, +1] //lateral movement forward and back

            //world space to local space; inverse is the key here
            Vector3 localDir = foot.transform.parent.InverseTransformDirection(input);
            

            float x = lateral * localDir.x;
            float z = lateral * localDir.z;


            //1 = forward vector
            //|-1| = backwards
            //0 = strafing
            float alignment = Mathf.Abs(Vector3.Dot(localDir, Vector3.forward));

            if (y < 0) y = 0;
            //adds offset
            //y += .177f;

            foot.SetPositionOffset(new Vector3(x, y, z), footSeperateAmount * alignment);
        };

        walkTime += Time.deltaTime * input.sqrMagnitude * walkFootSpeed;

        

        moveFoot.Invoke(walkTime, footLeft);
        moveFoot.Invoke(walkTime + Mathf.PI, footRight);
    }



}
