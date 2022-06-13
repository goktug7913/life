using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : LifeBase
{
    // This is the parent plant class.
    // It contains all the basic functions for all plants.

    public State state; // The current state of the plant.

    void Start()
    {
        base.Start();       // Call the base class's Start function.
        genus = Genus.Plantae;
    }

    void Update()
    {
        base.Update();
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
}