using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBaseV2 : MonoBehaviour
{
    // Parent class for all life objects.
    // Contains all the basic functions for all life objects.
    // Is meant to be inherited by other classes.
    public int creatureId;
    public Genus genus = Genus.na;
    public Sex sex = Sex.na;
    public int speciesId = 0;
    public int age = 0; // Age of the creature
    public int generation = 0; // Generation of the creature (in the context of bloodline)
    public bool isGenesis = false;

    // DNA will be an array of floats, each representing a gene
    // The length of the array will be the number of genes in the organism.
    // Length can vary depending on the organism.

    // The genes will be used to determine the behaviour and attributes of the organism.

    /*
    @goktug7913
    We should implement some sort of management system to handle
    which genes affect which attributes, because right now the genes
    are hardcoded for attributes, some reside in this class, and some
    reside in the child class.

    This will be a problem for the future, because we will need to
    be able to add new genes to the organism.
    */

    public float[] dna;
    public int dnaLength = 20;
    public float dnaMin = 0; // Minimum value for a gene
    public float dnaMax = 1; // Maximum value for a gene
    public float mutationRate = 0.01f; // Chance of mutation per gene
    public float mutationAmount = 0.1f; // Amount of mutation per gene
    public LifeBaseV2 otherParent; // Other parent of the offspring

    public float healthCoefficient = 10f; // Coefficient for the health of the creature
    public float health = 0; // All life will have a health value.
    public float maxHealth = 0; // The maximum possible health of the creature.
    public float healthRegen = 0; // The amount of health to regenerated per second.
    public bool canRegenHealth = false; // Whether or not the creature can regenerate health.

    // Common attributes to all life.
    public float mass = 1; // in kg
    public bool isAlive = true;
    public bool canReproduce = true;
    public bool canMove = false;
    public bool canAttack = false;
    public bool canBeAttacked = true;
    public bool canBeEaten = true;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    protected virtual void Start()
    {
        creatureId = GetInstanceID();
        
        if(isGenesis){
            // These are only called if the creature is spawned as initial population.
            GenerateDnaRandom();
            GenerateSpeciesId();
        }

        CalculateSex();
        InitStats();
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateHealth();
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    protected virtual void FixedUpdate()
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

    void GenerateSpeciesId()
    {
        speciesId = UnityEngine.Random.Range(1,9999);
    }

    void GenerateDnaRandom(){
        // For now we will generate random floats
        dna = new float[dnaLength];

        for (int i = 0; i < dnaLength; i++){
            dna[i] = UnityEngine.Random.Range(dnaMin, dnaMax); // terrible..
        }
        Debug.Log("Generated random DNA for: " + gameObject.name + " with length: " + dnaLength);
    }

    void InitStats(){
        // Initialize all stats here.
        // This will be called after the object is instantiated.
        float _mass = (dna[2]+dna[3]) / 2;
        mass = _mass * dna[4];
        
        maxHealth = mass * healthCoefficient;
        health = maxHealth;
    }

    // Function to calculate the health of the creature.
    void UpdateHealth(){
        if(canRegenHealth){
            health += healthRegen * Time.deltaTime;
        }
        if(health > maxHealth){
            health = maxHealth;
        }
        if(health <= 0){
            HandleDeath(); // ded :'(
        }
    }

    void HandleDeath(){
        // Log with id of the creature.
        Debug.Log("Creature " + gameObject.GetInstanceID() + " has died.");

        isAlive = false; // Not alive anymore.
        canReproduce = false; // We can't reproduce anymore.    
        canMove = false; // We can't move anymore.
        canAttack = false; // We can't attack anymore.
        canBeAttacked = false; // (Assumption) We can't be attacked anymore.
        canBeEaten = true; // We can be eaten in this state. (Not recommended :P)
        canRegenHealth = false; // We can't regenerate health anymore.
    }

    void CalculateSex(){
        // Sex is determined by DNA
        // Average the first 2 genes, if smaller than 0.5 its a male, if larger female
        float x = (dna[0] + dna[1])/2;
        if (x < .5f){sex = Sex.male;}
        else if (x > .5f){sex = Sex.female;}
        // !!!! There's the problem of x = 0.5 !!!!
    }

    float[] GenerateOffspringDna(){
        // This function will run on a female specimen
        float[] newDna = new float[dnaLength];

        for (int i = 0; i < dnaLength; i++){
            // Modulus 2 to alternate the parent genes (mother if even, father if odd) ,
            
            // We are also going to mutate the offspring by a small amount.
            // The mutation is applied by having a coefficient while passing on the float values.

            if (i % 2 == 0)
            {
                newDna[i] = this.dna[i] * (1 + UnityEngine.Random.Range(-mutationAmount, mutationAmount));
            }
            else
            {
                newDna[i] = otherParent.dna[i] * (1 + UnityEngine.Random.Range(-mutationAmount, mutationAmount));
            }
        }
        
        return newDna;
    }
}