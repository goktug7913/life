using System;
using UnityEngine;
using Creature.V3;
using Random = UnityEngine.Random;

public class SimulationManager : MonoBehaviour
{
    GameObject[] _trackedCreatures;
    
    // These need to be set from the Editor.
    [SerializeField] Mesh defaultMesh;
    [SerializeField] Material defaultMaterial;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.onSpawnOffspring += SpawnLife;
        EventManager.current.onSpawnGenesis += SpawnLife;
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update is called once per frame
    void Update()
    {
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // Cleanup
    void OnDestroy()
    {
        //Unsubscribe from all events
        
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // Custom methods and definitions go here.
    GameObject ConstructLife(float[] dna)
    {
        // I don't want to create a Life Prefab so this is how we initialize
        var life = new GameObject();
        var _base = life.AddComponent<LifeBaseV3>();

        // Set name and tag
        life.name = "Life " + life.gameObject.GetInstanceID();

        // Pass the base assets to the new object...
        _base.mesh = defaultMesh;
        _base.material = defaultMaterial;
        
        _base.genetics.SetDna(dna);
        
        return life;
    }
    
    public void SpawnLife(Vector3 position)
    {
        GameObject life = ConstructLife(GenerateDnaRandom(0,1,20));
        life.transform.position = position;
        Debug.Log("Spawned LifeV3 with genesis DNA");
    }
    
    public void SpawnLife(Vector3 position, float[] dna)
    {
        GameObject life = ConstructLife(dna);
        life.transform.position = position;
        Debug.Log("Spawned LifeV3 with the provided DNA");
    }

    static float[] GenerateDnaRandom(float min,float max, int length){
        float[] dna = new float[length];

        for (int i = 0; i < 20; i++){
            dna[i] = Random.Range(min, max); // terrible..
        }
        return dna;
    }
}
