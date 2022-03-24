using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//forces unity to have a catch for runtime errors
[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour
{
    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float walkFootSpeed = 2;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    public float walkSpreadX = .2f;
    private CharacterController pawn;

    void Start()
    {
        pawn = GetComponent<CharacterController>();    
    }

    
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * v + transform.right * h;

        if (move.sqrMagnitude > 1) move.Normalize();

        //applies delta movement times public speed
        pawn.SimpleMove(move * speed);

        //animate feet?
        AnimateWalk();

    }

    //makes a new green thing that can become a data type of
    delegate void MoveFoot(float time, float x, FootRaycast foot);

    void AnimateWalk()
    {
        //goal: Move y axis for up and z axis for forward
        //don't repeat yourself!

        //anon function
        MoveFoot moveFoot = (t, x, foot) =>
        {
            //floats for up and down and forward and back
            float y = Mathf.Cos(t) * walkSpreadY;
            float z = Mathf.Sin(t) * walkSpreadZ; //[-1, +1]

            if (y < 0) y = 0;
            //adds offset
            y += .177f;

            foot.transform.localPosition = new Vector3(x, y, z);
        };

        float t = Time.time * walkFootSpeed;

        moveFoot.Invoke(t,-walkSpreadX, footLeft);
        moveFoot.Invoke(t+ Mathf.PI, walkSpreadX, footRight);
    }



}
