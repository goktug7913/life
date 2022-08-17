using System.Collections.Generic;
using UnityEngine;
using Creature.V3;
using Random = UnityEngine.Random;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current; // Singleton
    
    public List<GameObject> _trackedCreatures;
    
    // These need to be set from the Editor.
    [SerializeField] Mesh defaultMesh;
    [SerializeField] Material defaultMaterial;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.OnSpawnOffspring += SpawnLife;
        EventManager.current.OnSpawnGenesis += SpawnLifeGenesis;
        
        _trackedCreatures = new List<GameObject>();
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
        EventManager.current.OnSpawnOffspring -= SpawnLife;
        EventManager.current.OnSpawnGenesis -= SpawnLifeGenesis;
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
        
        _trackedCreatures.Add(life);
        return life;
    }

    void SpawnLifeGenesis(Vector3 position)
    {
        GameObject life = ConstructLife(GenerateDnaRandom(0,1,30));
        life.transform.position = position;
        life.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        Debug.Log("Spawned LifeV3 with genesis DNA");
    }

    void SpawnLife(Vector3 position, float[] dna)
    {
        GameObject life = ConstructLife(dna);
        life.transform.position = position;
        life.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        Debug.Log("Spawned LifeV3 with the provided DNA");
    }

    static float[] GenerateDnaRandom(float min,float max, int length){
        float[] dna = new float[length];

        for (int i = 0; i < length; i++){
            dna[i] = Random.Range(min, max); // terrible..
        }
        return dna;
    }
}
