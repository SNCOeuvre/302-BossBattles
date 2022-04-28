using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    //gives hp to enemy
    float hp = 100;


    public void TakeDamage(float amt)
    {
        if (hp < 0) return; //prevent ThingGonnaDie calls before destroyed object remove
        if (amt < 0) amt = 0;
        hp -= amt;
        //broadcastmessage check the function on every script it can check
        if (hp < 0) BroadcastMessage("ThingGonnaDie");

    }

    void ThingGonnaDie()
    {
        //does nothing here...
    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
