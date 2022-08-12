using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public bool use_rb_movement = true; // Development variable for switching between RigidBody movement and GameObject movement.
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public Vector3 wanderTarget;
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public AnimalV2 mateTarget;
    public bool isMating = false;
    public bool isPregnant = false;
    public float gestationTime = 500f;
    public float gestationProgress = 0f;
    public bool debug_mateFlag = false;

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
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        genus = Genus.Animalia;

        infoCard = GetComponentInChildren<InfoCard>();
        infoCard.Attach(this);
        GenerateAttributes();
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    void Update()
    {
        base.Update();
        
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    void FixedUpdate()
    {
        base.FixedUpdate(); // Call the base class's FixedUpdate function.
        TickStats();        // We update the stats before we do anything else.
        DetermineState();   // We determine the state of the creature.
        ActOnState();       // We do whatever the creature is supposed to do in the current state.
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    void TickStats()
    {
        // We can extract this bit to its own function later on, like gestation.
        //hunger += Time.deltaTime/10f; // slow down the hunger.
        if (hunger > maxHunger){
            // Clamp to maxHunger and start losing health.
            hunger = maxHunger;
            health -= Time.deltaTime/10f; // slow down the health loss.
        } 
    
        if(isPregnant){TickGestation();}
    }

    void ActOnState()
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
                    state = State.SeekMate;
                }else {
                    // We will move around randomly if we can't find a mate.
                    Wander();
                }
                break;

            case State.SeekMate:
                SeekMate();
                break;

            case State.FindFood:
                FindFood();
                break;

            case State.Eat:
                Eat();
                break;

            default:
                Debug.LogError("State not defined for animal: " + gameObject.name + " - State: " + state);
                break;
        }
    }

    void DetermineState()
    {
        if (health <= 0)
        {
            state = State.Die;
            return;
        }
        // if hungry, find food
        if (hunger > maxHunger/2) // @goktug7913: Threshold is hardcoded for now.
        {
            state = State.FindFood;
            return;
        }
        if (mateTarget != null)
        {
            state = State.SeekMate;
            return;
        }
        // if not hungry and has no mate, find mate
        else if (hunger < maxHunger/4
                && mateTarget == null
                && debug_mateFlag == false) // Debug check to only mate once
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
        Wander, SeekMate, FindMate, Mate, SeekFood, FindFood, Eat, Die
    }

    void Move(Vector3 target)
    {
        if (use_rb_movement){
            rb_Move(target);
        }else {
            transform_Move(target);
        }
    }

    private void transform_Move(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        transform.Translate(direction * Time.deltaTime * speed);
    }

    void rb_Move(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        rb.AddForce(direction * Time.deltaTime * speed);
    }

    void Wander(){
        // This will be the wander function for the animal.
        // We will use the wander target to determine where the animal will move.
    
        float distance = Vector3.Distance(transform.position, wanderTarget);

        // Check to see if we need a new wander target.
        if( wanderTarget == null ||
            wanderTarget == Vector3.zero ||
            distance > visionRange ||
            distance <= 0.5f )
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

    void FindFood(){

    }

    void Eat(){

    }

    Vector3 GetWanderTarget(){
        // This will use the sight system to get a wander target later on.
        // For now we will get a random point in vision radius.
        Vector3 wanderTarget = transform.position + UnityEngine.Random.insideUnitSphere * visionRange;
        wanderTarget.y = 0.5f; // We will only use the x and z coordinates. (Y will make it floaty.)
        return wanderTarget;
    }

    bool FindMate()
    {
        // This will be the seek mate function for the animal.
        // We will use the mate target to determine where the animal will move.
        
        // Create an overlap sphere to find the mate.
        Collider[] colliders = Physics.OverlapSphere(transform.position, visionRange);
        foreach(Collider collider in colliders)
        {
            // Check to see if the collider is a mate.
            if(collider.gameObject != this.gameObject
            && collider.gameObject.GetComponent<AnimalV2>() != null
            && collider.gameObject.GetComponent<AnimalV2>().sex != sex
            // For now we will check if the target already has a mate.
            // Later on, we'll change this to a boolean which is determined by DNA
            && (
                collider.gameObject.GetComponent<AnimalV2>().mateTarget == null ||
                collider.gameObject.GetComponent<AnimalV2>().mateTarget == this
                )
            )
            {
                // We found an available mate.
                mateTarget = collider.gameObject.GetComponent<AnimalV2>();
                return true;
            }
        }
        return false; // no mate found.
    }

    void SeekMate()
    {
        // Check distance to mate target.
        float distance = Vector3.Distance(transform.position, mateTarget.transform.position);
        Debug.Log("SeekMate - Distance: " + distance);
        if (distance > 3f){
            // We are not close enough to the mate.
            // Move towards the mate.
            Move(mateTarget.transform.position);
            Debug.Log(creatureId + " move to mate");
        }
        else if(distance <= 5f){
            // We are close enough to the mate.
            // We will mate.
            Debug.Log(creatureId + " try to mate");
            TryMating();
        }
    }

    void TryMating(){
        // Check to see if we can mate.
        if(mateTarget.AcceptMate(this) && isMating == false){
            // We can mate.
            if (this.sex == Sex.female)
            {
                // Female starts gestation
                StartGestation();
                isMating = true;
            } else {
                // The other mate is female. We don't have
                // child-bearing males for now
                // We should call the relevant functions on the partner

                mateTarget.otherParent = this; // I think this is not set at this point.
                mateTarget.StartGestation();
                mateTarget.isMating = true;
            }

        mateTarget.mateTarget = null; // Probably bad place to handle this?
        mateTarget = null;
        }
    }

    void StartGestation(){
        // We will start the gestation period.
        isPregnant = true;
    }

    void TickGestation(){
        // This will be the gestation function for the animal.
        gestationProgress += Time.deltaTime;
        if(gestationProgress >= gestationTime){
            // We are ready
            SpawnOffspring();
            gestationProgress = 0;
            isPregnant = false;
        }
    }

    AnimalV2 SpawnOffspring()
    {
        if (debug_mateFlag == true){
            // Jank fix to prevent spawning every frame.
            Debug.Log("Creature: " + creatureId + " already spawned offspring, cannot mate again.");
            return null;
        }

        // The new animal object is created here.
        GameObject offspring = Instantiate( Resources.Load("Prefabs/Animal"),
                                            gameObject.transform.position,
                                            Quaternion.identity) as GameObject;

        AnimalV2 animal = offspring.GetComponent<AnimalV2>();

        animal.generation = CalculateOffspringGeneration();

        debug_mateFlag = true; // Debug check to only mate once
        mateTarget.debug_mateFlag = true; // Debug check to only mate once

        Debug.Log("Spawned offspring with ID: " + animal.creatureId + " Parents: " + this.creatureId + " and " + mateTarget.creatureId);
        return animal;
    }

    bool AcceptMate(AnimalV2 requester)
    {
        // This will be the accept mate function for the animal.
        if (
            base.canReproduce       &&
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

    void GenerateAttributes()
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

    void OnDrawGizmos(){
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

            case State.SeekMate:
                DrawMovementDebug(mateTarget.transform.position);
                break;

            default:
                Debug.Log("Cannot draw debug for unknown state on animal: " + gameObject.name + " - State: " + state);
                break;
        }
    }

    void DrawMovementDebug(Vector3 target)
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawLine(transform.position, target);
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawWireCube(target, new Vector3(1, 1, 1));
    }
}
