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

    // Attack system
    // TODO
    //- - - - - - - -

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();       // Call the base class's Start function.
        genus = Genus.Animalia;
        state = State.Idle;
        CalculateMass();
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();      // Call the base update function.
        SensoryInput();     // Update the sensory system.
        DecideState();      // Update the current state.
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

    void SensoryInput(){
        // TODO
    }

    void DecideState(){
        // TODO
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
            case State.LookingForFood:
                // The animal is looking for food.
                // It will check the known food locations first.

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

        // Do not rotate vertically.
        lookRotation.x = 0;

        // Move the animal in the direction of the wander target.
        transform.position += wanderDirection.normalized * speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnRate);
    }

    Vector3 GetWanderTarget(){
        // This will use the sight system to get a wander target later on.
        // For now we will get a random point in vision radius.
        Vector3 wanderTarget = transform.position + Random.insideUnitSphere * visionRange;
        wanderTarget.y = 0.5f; // We will only use the x and z coordinates. (Y will make it floaty.)
        return wanderTarget;
    }

    bool RememberFoodLoc(){
        // We can only remember if we have room for more food locations.
        if(foodLocLimit > foodLocations.Length){
            // We have room for more food locations.
            // We will remember the food location.
            return true;
        }
        return false;
        }

    bool RememberEnemyLoc(){
        return false;
        }

    bool RememberMateLoc(){
        return false;
        }

    bool ForgetFoodLoc(){
        return false;
        }

    bool ForgetEnemyLoc(){
        return false;
        }

    bool ForgetMateLoc(){
        return false;
        }

    void CalculateMass(){
        // This will calculate the mass of the animal.
        // TODO
    }

    void CalculateSpeed(){
        // This will calculate the speed of the animal.
        // TODO
    }

    void CalculateVision(){
        // This will calculate the vision range of the animal.
        // TODO
    }

    void CalculateHearing(){
        // This will calculate the hearing range of the animal.
        // TODO
    }

    void OnDrawGizmos(){
        // This will draw the vision range and hearing range of the animal.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
        if (wanderTarget != null && wanderTarget != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, wanderTarget);
            Gizmos.DrawWireCube(wanderTarget, new Vector3(1, 1, 1));
        }
    }
}