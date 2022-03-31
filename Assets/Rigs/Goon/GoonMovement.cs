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
        Walk
    }

    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float walkFootSpeed = 2;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    public float walkSpreadX = .2f;
    private CharacterController pawn;
    private Mode mode = Mode.Idle;
    private Vector3 input;
    private float walkTime;
    private Camera cam;

    void Start()
    {
        pawn = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    
    void Update()
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
        //applies delta movement times public speed
        pawn.SimpleMove(input * speed);

        //animate feet?
        Animate();

    }

    void Animate()
    {
        switch (mode)
        {
            case Mode.Idle:
                AnimateIdle();
                break;
            case Mode.Walk:
                AnimateWalk();
                break;
        }
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

            if (y < 0) y = 0;
            //adds offset
            //y += .177f;

            foot.SetPositionOffset(new Vector3(x, y, z));
        };

        walkTime += Time.deltaTime * input.sqrMagnitude * walkFootSpeed;

        

        moveFoot.Invoke(walkTime, footLeft);
        moveFoot.Invoke(walkTime + Mathf.PI, footRight);
    }



}
