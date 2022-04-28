using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameProjectile : MonoBehaviour
{
    //makes speed a property instead of hardcoded
    public float speed = 10;

    //determines how long flame lives
    public float lifeSpan = 1;

    public float fireDamage = 10;

    void Start()
    {

        //gets the body for volume purposes
        Rigidbody body = GetComponent<Rigidbody>();

        //uses physics engine to launch the projectile
        body.AddForce(transform.forward * speed, ForceMode.VelocityChange);

        //randomizes the lifespan
        lifeSpan = Random.Range(.25f, .5f);
    }

    private void Update()
    {
        //ticks down the lifespan of particle object
        lifeSpan -= Time.deltaTime;
        //destroys the object if it has ran it's course
        if (lifeSpan <= 0) Destroy(gameObject);
    }

    //reference to the collider of another direct and if can damage
    private void OnTriggerEnter(Collider collider)
    {

        var c = collider.GetComponent<Damagable>();
        if (c != null)
        {
            c.TakeDamage(fireDamage); //enemy takes damage
            Destroy(gameObject); //specifically the projectile

        }
    }

}
