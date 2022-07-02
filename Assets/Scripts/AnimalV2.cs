using UnityEngine;

public class AnimalV2 : LifeBaseV2
{
    public float speed = 1f;
    public float acceleration = 0f;
    public float deceleration = 0f;
    public float maxSpeed = 0f;
    public float turnRate = 2f;
    public float visionRange = 5f;
    
    public Vector3 wanderTarget;
    
    public AnimalV2 mateTarget;
    public bool isMating = false;
    public float gestationTime = 0f;
    public float gestationProgress = 0f;
    public bool debug_mateFlag = false;

    public State state;

    public float hunger;
    public float maxHunger;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        genus = Genus.Animalia;

        GenerateAttributes();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        DetermineState();
        ActOnState();
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
                break;
        }
    }

    void DetermineState()
    {
        if (health <= 0)
        {
            state = State.Die;
        }
        // if hungry, find food
        if (hunger > maxHunger/2) // @goktug7913: Threshold is hardcoded for now.
        {
            state = State.FindFood;
        }
        if (mateTarget != null)
        {
            state = State.SeekMate;
        }
        // if not hungry and has no mate, find mate
        else if (hunger < maxHunger/4
        && mateTarget == null
        && debug_mateFlag == false) // Debug check to only mate once
        {
            state = State.FindMate;
        }
        // if not hungry and not mating, wander
        else
        {
            state = State.Wander;
        }
    }

    public enum State {
        Wander, SeekMate, FindMate, Mate, SeekFood, FindFood, Eat, Die
    }

    void Move(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        transform.Translate(direction * Time.deltaTime * speed);
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
            if(collider.gameObject.GetComponent<AnimalV2>() != null
            && collider.gameObject.GetComponent<AnimalV2>().sex != sex
            // For now we will check if the target already has a mate.
            // Later on, we'll change this to a boolean which is determined by DNA
            && collider.gameObject.GetComponent<AnimalV2>().mateTarget == null)
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
        if (distance > 1f){
            // We are not close enough to the mate.
            // Move towards the mate.
            Move(mateTarget.transform.position);
        }
        else{
            // We are close enough to the mate.
            // We will mate.
            TryMating();
        }
    }

    void TryMating(){
        // Check to see if we can mate.
        if(mateTarget.AcceptMate(this)){
            // We can mate.
            // We will mate.
            if (this.sex == Sex.female)
            {
                // Create an offspring creature object here
                // We ignore gestation for now

                SpawnOffspring();
            } else {
                // The other mate is female. We don't have
                // child-bearing males for now
                // We should call the relevant functions on the partner

                mateTarget.SpawnOffspring();
            }

        mateTarget.mateTarget = null; // Probably bad place to handle this?
        mateTarget = null;
        }
    }

    AnimalV2 SpawnOffspring()
    {
        if (debug_mateFlag == true){
            // Jank fix to prevent spawning every frame.
            return null;
        }

        GameObject offspring = Instantiate(Resources.Load("Prefabs/Animal"), gameObject.transform.position, Quaternion.identity) as GameObject; // transform might be wrong, wrote on laptop
        AnimalV2 animal = offspring.GetComponent<AnimalV2>();

        debug_mateFlag = true; // Debug check to only mate once

        return animal;
    }

    bool AcceptMate(AnimalV2 requester)
    {
        // This will be the accept mate function for the animal.
        if (
            base.canReproduce &&
            requester.canReproduce &&
            requester.sex != sex // we ignore homosexual/asexual repro. for now (could be fun tho)
        )
        {
            // We can mate.
            base.otherParent = requester; // base needs a ref for dna generation
            return true;
        }
        // We cannot mate with this requester.
        return false;
    }

    void GenerateAttributes()
    {
        // First 5 genes are used in LifeBaseV2
        
        // We will use avg of 6 and 7 for speed
        float _speed = (dna[5] + dna[6])/2;
        
        // We will use avg*5 of 5 and 6 for vision range
        float _visionRange = (dna[7] + dna[8])/2;

        speed = _speed*3;
        visionRange = _visionRange*5;

        // max hunger is half of max health
        maxHunger = maxHealth/2;
    }

    void OnDrawGizmos(){
        // This will draw the vision range and hearing range of the animal.
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, visionRange);

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
