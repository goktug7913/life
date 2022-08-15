using System.Collections;
using Creature.V3;
using UnityEngine;
using UnityEngine.Serialization;

public class SimulationManager : MonoBehaviour
{
    GameObject[] _trackedCreatures;
    
    [SerializeField] Mesh defaultMesh;
    [SerializeField] Material defaultMaterial;
    
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
    GameObject ConstructLife(float[] dna)
    {
        // I don't want to create a Life Prefab so this is how we initialize
        var life = new GameObject();
        var _base = life.AddComponent<LifeBaseV3>();

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
    }
    
    public void SpawnLife(Vector3 position, float[] dna)
    {
        GameObject life = ConstructLife(dna);
        life.transform.position = position;
    }

    static float[] GenerateDnaRandom(float min,float max, int length){
        float[] dna = new float[length];

        for (int i = 0; i < 20; i++){
            dna[i] = Random.Range(min, max); // terrible..
        }
        return dna;
    }
}
