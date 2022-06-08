using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LifeBase
{
    // This is the parent animal class.
    // It contains all the basic functions for all animals.
    // Is meant to be inherited by other classes.
    
    // Sensory system for awareness of other life objects.
    float visionRange = 0;
    float visionAngle = 0;
    float hearingRange = 0;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        genus = Genus.Animalia;
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // Custom methods and definitions go here.

    enum State{
        /* We will have a state machine for the animal, which will be used to determine what the animal is doing.
        As this is the parent class for all animals, we will have many possible states,
        but not all of them will be used.
        This enumeration should be expanded and edited to fit the parent class and the simulation system. */
        Idle, Exploring, Attacking, Defending, Dying, Dead, Reproducing,
        Fleeing, Following, Stalking, FollowingLeader, ChasingPrey,
        LookingForMate, LookingForFood, LookingForWater, LookingForHome,
        Eating, Drinking, Sleeping,
    }
}
