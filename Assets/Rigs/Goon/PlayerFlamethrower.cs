using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlamethrower : MonoBehaviour
{
    //reference to the flamethrower game object
    public FlameProjectile projectilePrefab;
    //references the particlesystems
    public ParticleSystem fire;

    bool isTurnedOn = false;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        //grab the click button, to access it
        bool wantsToUse = Input.GetButton("Fire1");

        //allows for cooldown
        isTurnedOn = wantsToUse;

        if (isTurnedOn)
        {
            //creates spawnpoint
            Vector3 spawnPoint = transform.position + transform.forward * 1.5f + transform.up * 1.5f;

            //makes flamethrower and where it spawns
            Instantiate(projectilePrefab, spawnPoint, transform.rotation);
        }


        var emission = fire.emission;
        
        
        emission.rateOverTime = isTurnedOn ? 20 : 0;
    }
}
