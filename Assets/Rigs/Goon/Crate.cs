using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{

    //is called for so when it dies, it can die and be destroyed
    void ThingGonnaDie()
    {
        print("I drop loot now...");
        Destroy(gameObject);
    }
}
