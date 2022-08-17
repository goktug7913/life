using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LifeBase
{
    // This is the parent animal class.
    // It contains all the basic functions for all animals.
    
    public State state; // The current state of the animal. (e.g. wandering, attacking, etc.)

    // Neural data
    public int foodLocLimit = 0; // The maximum number of food locations the animal can remember.
    public int enemyLocLimit = 0;
    public int mateLocLimit = 0;
    public Vector3[] foodLocations; // Memory of food locations.
    public Vector3[] enemyLocations; // Memory of enemy locations.
    public Vector3[] mateLocations; // Memory of mate locations.
    public Vector3 wanderTarget; // The target location for the animal to wander to.
    public Animal mateTarget; // The target animal for the animal to mate with.
    public Animal attackTarget; // The target location for the animal to attack.
    public Vector3 fleeTarget; // The target location for the animal to flee to.

    // Behaviour data
    public float hunger;
    public float maxHunger;
    public float thirst;
    public float maxThirst;
    public float confidence;
    public float fear;
    public float digestionProgress;
    public float digestionSpeed;

    // Reproduction data
    public float gestationTime;
    public float gestationProgress;

    // Sensory system for awareness of other life objects.
    public float visionRange = 5f;
    public float visionAngle = 0f;
    public float hearingRange = 0f;

    // Movement system
    public float speed = 1f;
    public float acceleration = 0f;
    public float deceleration = 0f;
    public float maxSpeed = 0f;
    public float turnRate = 2f;
    
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // Custom methods and definitions go here.

    public enum State{
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