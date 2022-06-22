using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    void Update()
    {
        // Spawn a pair of animals on right click.
        if (Input.GetMouseButtonDown(1))
        {
            SpawnNewSpeciesPair(LifeBase.Genus.Animalia);
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // Custom methods and definitions go here.

    void SpawnNewSpeciesPair(LifeBase.Genus genus){
        /* This function spawns a new species pair of given genus
        around the mouse cursor.*/

        // Get the mouse position in world space.
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        // Move the Y position to the ground.
        worldPos.y = 3;

        // Spawn the new species pair.
        switch (genus){
            case LifeBase.Genus.Animalia:
                GameObject a1 = Instantiate(Resources.Load("Prefabs/Animal"), worldPos, Quaternion.identity) as GameObject;
                a1.GetComponent<AnimalV2>().isGenesis = true;
                break;
            case LifeBase.Genus.Plantae:
                // TODO
                Debug.Log("Cannot create plant pair: not implemented yet.");
                break;
            case LifeBase.Genus.Fungi:
                // TODO
                Debug.Log("Cannot create fungus pair: not implemented yet.");
                break;
        }
    }

}
