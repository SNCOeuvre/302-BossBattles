using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionStickyFeet : MonoBehaviour
{
    public Transform rayCastSource;
    public AnimationCurve curveStepVerticle;
    public float raycastLength = 2;
    public float maxDistanceBeforeMove = 1;
    private Quaternion startingRotation;
    private Quaternion groundRotation;
    private Quaternion previousGroundRotation;
    private Vector3 groundPosition;
    private Vector3 previousGroundPosition;
    private float animationLength = .25f;
    private float animationCurrentTime = 0;
    private bool isAnimating { 
        get
        {
            return (animationCurrentTime < animationLength);
        }
    
    }
    

    void Start()
    {
        startingRotation = transform.localRotation;
    }

    void Update()
    {
        if (isAnimating)
        {
            animationCurrentTime += Time.deltaTime;
            float p = Mathf.Clamp(animationCurrentTime / animationLength, 0, 1);

            float y = curveStepVerticle.Evaluate(p);

            //move position
            transform.position = AnimMath.Lerp(previousGroundPosition, groundPosition, p) + new Vector3(0, y, 0);
            //move rotation
            transform.rotation = AnimMath.Lerp(previousGroundRotation, groundRotation, p);
        }
        else
        {
            //if not animating, keep feet where they are supposed to be, planted
            transform.position = groundPosition;
            transform.rotation = groundRotation;

            //check distance to starting position, trigger animation
            Vector3 vToStarting = transform.position - rayCastSource.position;
            if (vToStarting.sqrMagnitude > maxDistanceBeforeMove * maxDistanceBeforeMove)
            {
                FindGround();
            }
        }
    }

    

    private void FindGround()
    {
        //Where the raycast starts
        Vector3 origin = rayCastSource.position + Vector3.up * raycastLength / 2;
        //where the raycast is shooting
        Vector3 direction = Vector3.down;

        //visualizes the ray, and keeps it out for a moment every frame | draw the ray in the scene:
        Debug.DrawRay(origin, direction * raycastLength, Color.yellow);

        //check for collision with ray:
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength))
        {
            //prepare animation values
            previousGroundPosition = groundPosition;
            previousGroundRotation = groundRotation;
            animationCurrentTime = 0;

            //finds ground position
            groundPosition = hitInfo.point;

            //uses parent rotation times the starting local rotation | converts starting rotation into world-space
            Quaternion worldNeutral = transform.parent.rotation * startingRotation;

            //rotation aligned with ground times the world rotation | finds ground rotation
            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;


            
        }
    }

}
