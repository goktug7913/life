using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBase : MonoBehaviour
{
    public Genus genus = Genus.na;
    public Sex sex = Sex.na; 
    public int age = 0; // Age of the creature
    public int generation = 0; // Generation of the creature (in the context of bloodline)

    // DNA will be an array of floats, each representing a gene
    // The length of the array will be the number of genes in the organism,
    // and the values of the genes will be between 0 and 1.
    // Length can vary depending on the organism.

    // The genes will be used to determine the behaviour and attributes of the organism.
    public float[] dna;

    public float health = 0; // All life will have a health value.
    public float maxHealth = 0; // The maximum possible health of the creature.
    public float healthRegen = 0; // The amount of health to regenerated per second.
    public bool canRegenHealth = false; // Whether or not the creature can regenerate health.

    // Common attributes to all life.
    public bool isAlive = true;
    public bool canReproduce = true;
    public bool canMove = false;
    public bool canAttack = false;
    public bool canBeAttacked = true;
    public bool canBeEaten = true;
    
    public float decayTime = 0; // Time in seconds before the body decays, will mainly be affected by the mass of the body.
    public float decayTimer = 0; // Time left before the body decays.
    public bool isDecaying = false; // Whether or not the body is decaying.
    public bool isDecayed = false; // Whether or not the body is decayed.

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    void Update()
    {
        
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // Custom methods and definitions go here.
    public enum Genus{
        Animalia,
        Plantae,
        Fungi,
        na,
        // Note: More in the future?
    }

    // All life will have a sex, even if it doesn't classify as male or female,
    // it will enumerate to NA.
    public enum Sex{
        male,
        female,
        na,
    }

    // Function to calculate the health of the creature.
    void CalculateHealth(){
        if(canRegenHealth){
            health += healthRegen * Time.deltaTime;
        }
        if(health > maxHealth){
            health = maxHealth;
        }
    }

    void HandleDeath(){
        // Log with id of the creature.
        Debug.Log("Creature " + gameObject.GetInstanceID() + " has died.");

        isAlive = false; // Not alive anymore.
        isDecaying = true; // It will start decaying.
        canReproduce = false; // We can't reproduce anymore.    
        canMove = false; // We can't move anymore.
        canAttack = false; // We can't attack anymore.
        canBeAttacked = false; // (Assumption) We can't be attacked anymore.
        canBeEaten = true; // We can be eaten in this state. (Not recommended :P)
        canRegenHealth = false; // We can't regenerate health anymore.
        decayTimer = decayTime; // We start decaying.
    }

    void DecayTicker(){
        if(decayTimer > 0){
            decayTimer -= Time.deltaTime;
        }
        if(decayTimer <= 0){
            isDecayed = true;
        }
    }

    void HandleDecay(){
        // Simple logic to handle decay.
        // Should be moved to a coroutine later, for performance reasons.
        if (isDecaying){
            DecayTicker();
        }
        if (isDecayed){
            // Log with id of the creature.
            Debug.Log("Creature " + gameObject.GetInstanceID() + " has decayed.");
            Destroy(gameObject);
        }
    }

}
