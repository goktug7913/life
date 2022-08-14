using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AnimalV2 : LifeBaseV2
{
    Rigidbody rb;
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public float speed = 1f;
    public float acceleration = 0f;
    public float deceleration = 0f;
    public float maxSpeed = 0f;
    public float turnRate = 2f;
    public float visionRange = 5f;
    public bool useRbMovement = false; // Development variable for switching between RigidBody movement and GameObject movement.
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public Vector3 wanderTarget;
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public AnimalV2 mateTarget;
    public bool isMating = false;
    public bool isPregnant = false;
    public bool hasMatedRecently = false;

    public float gestationProgress = 0f;
    public float gestationTime = 5f; // Make this based on DNA later.

    public float matingProgress = 0f;
    public float matingTime = 5f; // Make this based on DNA later.

    public float matingCooldownProgress = 0f;
    public float matingCooldownTime = 15f; // Make this based on DNA later.
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    // We're gonna have a list for the previous mates which rejected mating with us.
    // This will help us to avoid mating with the same mate twice.
    // Parameters for this feature should be generated from DNA later on.
    public List<AnimalV2> previousMates = new List<AnimalV2>();
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public State state;
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public float hunger;
    public float maxHunger;
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        genus = Genus.Animalia;

        infoCard = GetComponentInChildren<InfoCard>();
        infoCard.Attach(this);
        GenerateAttributes();

        // if male set color blue
        GetComponent<MeshRenderer>().material.color = sex switch
        {
            Sex.Male => Color.blue,
            Sex.Female => Color.magenta,
            _ => GetComponent<MeshRenderer>().material.color
        };
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        TickStats();        // We update the stats before we do anything else.
        DetermineState();   // We determine the state of the creature.
        ActOnState();       // We do whatever the creature is supposed to do in the current state.
        
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    private void TickStats()
    {
        TickHunger(); // Hunger never stops...
        if(isPregnant){TickGestation();}
        if(hasMatedRecently){TickMatingCooldown();}
    }

    private void TickHunger()
    {
        hunger += Time.deltaTime/50f; // slow down the hunger.
        if (!(hunger >= maxHunger)) return;
        // Clamp to maxHunger and start losing health.
        hunger = maxHunger;
        health -= Time.deltaTime/10f; // slow down the health loss.
    }

    private void TickMatingCooldown()
    {
        matingCooldownProgress += Time.deltaTime;
        if (!(matingCooldownProgress >= matingCooldownTime)) return;
        matingCooldownProgress = 0f;
        hasMatedRecently = false;
    }

    private void TickMating()
    {
        matingProgress += Time.deltaTime;
        if (!(matingProgress > matingTime)) return;
        matingProgress = 0f;
        isMating = false;
        isPregnant = true;
        gestationProgress = 0f;
    }

    private void ActOnState()
    {
        switch (state)
        {
            case State.Wander:
                Wander();
                break;

            case State.FindMate:
                if(FindMate())
                {
                    // If we found a mate, we will approach it.
                    state = State.GoToMate;
                }else {
                    // We will move around randomly if we can't find a mate.
                    Wander();
                }
                break;

            case State.GoToMate:
                SeekMate();
                break;

            case State.Mate:
                TickMating();
                break;
            case State.FindFood:
                FindFood();
                break;

            case State.Eat:
                Eat();
                break;

            case State.GoToFood:
            case State.Die:
            default:
                Debug.LogError("State not defined for animal: " + gameObject.name + " - State: " + state);
                break;
        }
    }

    private void DetermineState()
    {
        if (health <= 0)
        {
            state = State.Die;
            return;
        }
        // if hungry, find food
        if (hunger > maxHunger/2) // @goktug7913: Threshold is hardcoded for now.
        {
            //Commented to disable for development.
            //state = State.FindFood;
            return;
        }
        if (isMating)
        {
            state = State.Mate;
            return;
        }
        if (mateTarget != null && !hasMatedRecently)
        {
            state = State.GoToMate;
            return;
        }
        // if not hungry and has no mate, find mate
        else if (hunger < maxHunger/4
                && mateTarget == null
                && !hasMatedRecently)
        {
            state = State.FindMate;
            return;
        }
        // if not hungry and not mating, wander
        else
        {
            state = State.Wander;
            return;
        }
    }

    public enum State {
        Wander, GoToMate, FindMate, Mate, GoToFood, FindFood, Eat, Die
    }

    private void Move(Vector3 target)
    {
        if (useRbMovement){
            rb_Move(target);
        }else {
            transform_Move(target);
        }
    }

    private void transform_Move(Vector3 target)
    {
        var position = transform.position;
        Vector3 moveDirection = target - position;
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);

        // Do not rotate vertically.
        lookRotation.x = 0;

        // Move the animal in the direction of the wander target.
        position += moveDirection.normalized * (speed * Time.deltaTime);
        transform.position = position;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnRate);
    }

    private void rb_Move(Vector3 target)
    {
        // THIS IS EXPERIMENTAL CODE.
        // CAN BE USED BY CHANGING THE USE_RB_MOVEMENT FLAG.
        Vector3 direction = target - transform.position;
        direction.y = 0;
        rb.AddForce(direction * (Time.deltaTime * speed * 1000f));
    }

    private void Wander(){
        // This will be the wander function for the animal.
        // We will use the wander target to determine where the animal will move.
    
        float distance = Vector3.Distance(transform.position, wanderTarget);

        // Check to see if we need a new wander target.
        if( wanderTarget == Vector3.zero ||
            distance > visionRange ||
            distance <= 0.5f )
        {
            // We will generate a new wander target.
            wanderTarget = GetWanderTarget();
        }

        Move(wanderTarget);
    }

    void FindFood()
    {
    }

    void Eat()
    {
    }

    private Vector3 GetWanderTarget()
    {
        // This will use the sight system to get a wander target later on.
        // For now we will get a random point in vision radius.
        Vector3 target = transform.position + Random.insideUnitSphere * visionRange;
        target.y = 0.5f; // We will only use the x and z coordinates. (Y will make it floaty.)
        return target;
    }

    private bool FindMate()
    {
        // This will be the seek mate function for the animal.
        // We will use the mate target to determine where the animal will move.
        
        // Create an overlap sphere to find the mate.
        Collider[] colliders = Physics.OverlapSphere(transform.position, visionRange);
        foreach(var colliderObj in colliders)
        {
            // Check to see if the collider is a mate.
            if (colliderObj.gameObject == this.gameObject
                || colliderObj.gameObject.GetComponent<AnimalV2>() == null
                || colliderObj.gameObject.GetComponent<AnimalV2>().sex == sex ||
                // For now we will check if the target already has a mate.
                // Later on, we'll change this to a boolean which is determined by DNA
                !colliderObj.gameObject.GetComponent<AnimalV2>().canReproduce
                || (colliderObj.gameObject.GetComponent<AnimalV2>().mateTarget != null &&
                    colliderObj.gameObject.GetComponent<AnimalV2>().mateTarget != this)) continue;
            // We found an available mate.
            mateTarget = colliderObj.gameObject.GetComponent<AnimalV2>();
            return true;
        }
        return false; // no mate found.
    }

    private void SeekMate()
    {
        // Check distance to mate target.
        var distance = Vector3.Distance(gameObject.transform.position, mateTarget.gameObject.transform.position);
        switch (distance)
        {
            case > 3f:
                // We are not close enough to the mate.
                // Move towards the mate.
                Move(mateTarget.transform.position);
                return;
            case <= 4f:
                // We are close enough to the mate.
                // We will mate.
                Debug.Log(creatureId + " try to mate");
                TryMating();
                return;
        }
    }

    private void TryMating()
    {
        // Check to see if we can mate.
        if (hasMatedRecently || isMating != false || !mateTarget.AcceptMate(this)) return;
        // We can mate.
        if (this.sex == Sex.Female)
        {
            // Female starts gestation
            otherParent = mateTarget; // This needs to be refactored
            StartGestation();
            isMating = true;
            hasMatedRecently = true;
        }
        else
        {
            // The other mate is female. We don't have
            // child-bearing males for now
            // We should call the relevant functions on the partner

            mateTarget.otherParent = this; // I think this is not set at this point.
            mateTarget.StartGestation();
            mateTarget.isMating = true;
            mateTarget.hasMatedRecently = true;

            // We also need to handle our state.
            isMating = true;
            hasMatedRecently = true;
        }

        mateTarget.mateTarget = null; // Probably bad place to handle this?
        mateTarget = null;
    }

    void StartGestation(){
        // We will start the gestation period.
        isPregnant = true;
    }

    private void TickGestation(){
        // This will be the gestation function for the animal.
        gestationProgress += Time.deltaTime;
        if (!(gestationProgress >= gestationTime)) return;
        // We are ready to give birth.
        gestationProgress = 0;
        isPregnant = false;
        Invoke("SpawnOffspring", 1);
    }

    private AnimalV2 SpawnOffspring()
    {
        // The new animal object is created here.
        var offspring = Instantiate( Resources.Load("Prefabs/Animal"),
                                            gameObject.transform.position,
                                            Quaternion.identity) as GameObject;

        if (offspring == null) throw new InvalidOperationException();
        
        var animal = offspring.GetComponent<AnimalV2>();
        animal.generation = CalculateOffspringGeneration();

        Debug.Log("Spawned offspring with ID: " + animal.creatureId + " Parents: " + this.creatureId + " and " + mateTarget.creatureId);
        return animal;

    }

    bool AcceptMate(AnimalV2 requester)
    {
        // This will be the accept mate function for the animal.
        if (
            base.canReproduce       &&
            !isPregnant             &&
            !requester.isPregnant   &&
            requester.canReproduce  &&
            requester.sex != sex    // we ignore homosexual/asexual repro. for now (could be fun tho)
           )
        {
            // We can mate.
            base.otherParent = requester; // base class needs a reference for dna generation
            Debug.Log("Creature" + creatureId + " accepted mate with " + requester.creatureId);
            return true;
        }
        // We cannot mate with this requester.
        Debug.Log("Creature " + creatureId + "Cannot mate with " + requester.creatureId);
        return false;
    }

    private void GenerateAttributes()
    {
        // First 5 genes are used in LifeBaseV2
        
        // We will use avg of 6 and 7 for speed
        float _speed = (dna[5] + dna[6])/2;
        
        // We will use avg*5 of 5 and 6 for vision range
        float _visionRange = (dna[7] + dna[8])/2;

        speed = _speed*4;
        visionRange = _visionRange*25;

        // max hunger is half of max health
        maxHunger = maxHealth/2;
    }

    private void OnDrawGizmos(){
        // This will draw the vision range of the animal.
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // State related gizmos.
        switch (state)
        {
            case State.Wander:
                DrawMovementDebug(wanderTarget);
                break;

            case State.FindMate:
                DrawMovementDebug(wanderTarget);
                break;

            case State.GoToMate:
                if (mateTarget != null)
                {
                    DrawMovementDebug(mateTarget.transform.position);
                }
                break;

            case State.Mate:
            case State.GoToFood:
            case State.FindFood:
            case State.Eat:
            case State.Die:
            default:
                Debug.Log("Cannot draw debug for unknown state on animal: " + gameObject.name + " - State: " + state);
                break;
        }
    }

    private void DrawMovementDebug(Vector3 target)
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawLine(transform.position, target);
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawWireCube(target, new Vector3(1, 1, 1));
    }
}