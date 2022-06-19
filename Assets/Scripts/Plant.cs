using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : LifeBase
{
    // This is the parent plant class.
    // It contains all the basic functions for all plants.

    public List<Action> currentActions; // The current state of the plant.

    public float energy = 0; // The amount of energy the plant has.

    public float waterLevel = 0; // The current water level of the plant.
    public float maxWaterLevel = 0; // The maximum water level of the plant.
    public float nutrition = 0; // The current nutrition of the plant.
    public float maxNutrition = 0; // The maximum nutrition of the plant.
    
    public float growthRate = 0; // The rate at which the plant grows.
    public float growthProgress = 0; // The progress of the plant's growth.
    public float growthTime = 0; // The time it takes for the plant to grow.
    
    public float deathTime = 0; // The time it takes for the plant to die.
    public float deathProgress = 0; // The progress of the plant's death.   
    public float deathRate = 0; // The rate at which the plant dies.

    void Start()
    {
        base.Start();       // Call the base class's Start function.
        genus = Genus.Plantae;
    }

    void Update()
    {
        base.Update();
        ManageActions();
        UpdateState();
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public enum Action{
        /* Todo: Explain what the fuck this is*/
        Grow,
        Die,
        Photosynthesize,
        ProduceSeeds,
        DropSeeds,

    }

    void ManageActions()
    {
        // TODO
        //- - - - - - - -
    }

    void UpdateState()
    {
        // Go through the actions and execute them.
        for (int i = 0; i < currentActions.Count; i++)
        {
            switch (currentActions[i])
            {
                case Action.Grow:
                    Grow();
                    break;
                case Action.Die:
                    Die();
                    break;
                case Action.Photosynthesize:
                    Photosynthesize();
                    break;
                case Action.ProduceSeeds:
                    ProduceSeeds();
                    break;
                case Action.DropSeeds:
                    DropSeeds();
                    break;
            }
        }

    }

    void Grow(){
        // Spend energy to grow.
        // If the plant is at max size, stop growing.
    }

    void Die(){
        // TODO
    }

    void Photosynthesize(){
        // Spend nutrients and water to photosynthesize.
        nutrition -= 0.1f;
        waterLevel -= 0.1f;

        energy += 0.1f;
    }
    
    void ProduceSeeds(){
        // TODO
    }

    void DropSeeds(){
        // TODO
    }

    void AddAction(Action action){
        currentActions.Add(action);
    }

    void RemoveAction(Action action){
        currentActions.Remove(action);
    }

    void ClearActions(){
        currentActions.Clear();
    }
}