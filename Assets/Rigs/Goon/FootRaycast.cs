using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour
{
    /// <summary>
    /// The local space position of where the IK spawned
    /// </summary>
    private Vector3 startingPosition;

    
    // length of the raycast in meters
    public float raycastLength = 2;
    /// <summary>
    /// The local space rotation of where the IK spawned
    /// </summary>
    private Quaternion startingRotation;

    /// <summary>
    /// The world-space position of the ground above/below the foot IK.
    /// </summary>
    private Vector3 groundPosition;
    /// <summary>
    /// The world-space rotation for the foot to be aligned w/ground.
    /// </summary>
    private Quaternion groundRotation;

    /// <summary>
    /// The local space position to ease towards. This allows us to animate the position!
    /// </summary>
    private Vector3 targetPosition;

    void Start()
    {
        startingRotation = transform.localRotation;
        //sets it to wherever the foot is in the local position in the scene
        startingPosition = transform.localPosition;
    }

    
    void Update()
    {
        //FindGround();

        //ease towards target
        transform.localPosition = AnimMath.Ease(transform.localPosition, targetPosition, .01f);
    }

    public void SetPositionLocal(Vector3 p)
    {
        targetPosition = p;
    }

    public void SetPositionHome()
    {
        targetPosition = startingPosition;
    }

    public void SetPositionOffset(Vector3 p)
    {
        targetPosition = startingPosition + p; 
    }

    private void FindGround()
    {
        //Where the raycast starts
        Vector3 origin = transform.position + Vector3.up * raycastLength / 2;
        //where the raycast is shooting
        Vector3 direction = Vector3.down;

        //visualizes the ray, and keeps it out for a moment every frame | draw the ray in the scene:
        Debug.DrawRay(origin, direction * raycastLength, Color.yellow);

        //check for collision with ray:
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength))
        {

            //finds ground position
            groundPosition = hitInfo.point + Vector3.up * startingPosition.y;

            //uses parent rotation times the starting local rotation | converts starting rotation into world-space
            Quaternion worldNeutral = transform.parent.rotation * startingRotation;

            //rotation aligned with ground times the world rotation | finds ground rotation
            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;
        }
    }
}
