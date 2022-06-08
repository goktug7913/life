using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LifeBase
{
    // This is the parent animal class.
    // It contains all the basic functions for all animals.
    // Is meant to be inherited by other classes.
    
    public State state; // The current state of the animal. (e.g. wandering, attacking, etc.)

    // Sensory system for awareness of other life objects.
    public float visionRange = 0;
    public float visionAngle = 0;
    public float hearingRange = 0;

    // Movement system
    public float speed = 0;
    public float acceleration = 0;
    public float deceleration = 0;
    public float maxSpeed = 0;

    // Attack system
    // TODO
    //- - - - - - - -

    // Other attributes
    public Vector3 wanderTarget;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();       // Call the base class's Start function.
        genus = Genus.Animalia;
        state = State.Idle;
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();      // Call the base update function.
        StateMachine();     // Act on the current state.
    }
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

    void StateMachine(){
        // This will be the state machine for the animal.
        // It will determine what the animal is doing.
        // This will be expanded and edited to fit the parent class and the simulation system.
        switch(state){
            case State.Idle:
                // The animal is idle.
                // It will wander around.
                Wander();
                break;
            case State.Exploring:
                // The animal is exploring.
                // It will look for food and water.
                // It will also look for other animals.
                break;
            default:
                // State was not defined, we should log this.
                Debug.Log("State not defined for animal: " + gameObject.name + " - State: " + state);
                break;
        }
    }

    void Wander(){
        // This will be the wander function for the animal.
        // We will use the wander target to determine where the animal will move.
    
        float distance = Vector3.Distance(transform.position, wanderTarget);

        // Check to see if we need a new wander target.
        if( wanderTarget == null ||
            wanderTarget == Vector3.zero ||
            distance > visionRange ||
            distance <= 2.5f )
        {
            // We will generate a new wander target.
            wanderTarget = GetWanderTarget();
        }

        Vector3 wanderDirection = wanderTarget - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(wanderDirection);

        lookRotation.x = 0; // We don't want the animal to look up or down.

        // Move the animal in the direction of the wander target.
        transform.position += wanderDirection.normalized * speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    Vector3 GetWanderTarget(){
        // This will use the sight system to get a wander target later on.
        // For now we will get a random point in vision radius.
        Vector3 wanderTarget = transform.position + Random.insideUnitSphere * visionRange;
        wanderTarget.y = 0; // We will only use the x and z coordinates. (Y will make it floaty.)
        return wanderTarget;
    }

    void OnDrawGizmos(){
        // This will draw the vision range and hearing range of the animal.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
        Gizmos.DrawLine(transform.position, wanderTarget);
    }

}
