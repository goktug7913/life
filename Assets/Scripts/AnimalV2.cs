using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalV2 : LifeBaseV2
{
    public float speed = 1f;
    public float acceleration = 0f;
    public float deceleration = 0f;
    public float maxSpeed = 0f;
    public float turnRate = 2f;
    public Vector3 wanderTarget;
    public float visionRange = 5f;

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
        Wander();
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

    Vector3 GetWanderTarget(){
        // This will use the sight system to get a wander target later on.
        // For now we will get a random point in vision radius.
        Vector3 wanderTarget = transform.position + Random.insideUnitSphere * visionRange;
        wanderTarget.y = 0.5f; // We will only use the x and z coordinates. (Y will make it floaty.)
        return wanderTarget;
    }

    void SeekMate()
    {
        // This will be the seek mate function for the animal.
        // We will use the mate target to determine where the animal will move.
        // Get opposite sex of same genus in vision range.
        
    }

    void AcceptMate()
    {
        // This will be the accept mate function for the animal.
        // We will use the mate target to determine where the animal will move.
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
        if (wanderTarget != null && wanderTarget != Vector3.zero)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawLine(transform.position, wanderTarget);
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawWireCube(wanderTarget, new Vector3(1, 1, 1));
        }
    }
}
