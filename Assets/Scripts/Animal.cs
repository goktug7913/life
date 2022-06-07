using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LifeBase
{
    // This is the parent animal class.
    // It contains all the basic functions for all animals.
    // Is meant to be inherited by other classes.
    
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    void Start()
    {
        
        genus = Genus.Animalia;
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    void Update()
    {
        
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // Custom methods and definitions go here.
    
}
