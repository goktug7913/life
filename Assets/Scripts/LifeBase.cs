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
        na
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
    public void CalculateHealth(){
        if(canRegenHealth){
            health += healthRegen * Time.deltaTime;
        }
        if(health > maxHealth){
            health = maxHealth;
        }
    }
}
