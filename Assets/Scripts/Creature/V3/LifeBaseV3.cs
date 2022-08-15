using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Creature.V3
{
    public class LifeBaseV3 : MonoBehaviour
    {
        public Genetics genetics;
        Brain _brain;
        Movement _movement;
        Genitalia _genitalia;
        
        MeshFilter _meshFilter;
        MeshRenderer _renderer;
        MeshCollider _collider;
        Rigidbody _rigidbody;
        
        public Mesh mesh;
        public Material material;
        
        void Awake()
        {
            genetics = gameObject.AddComponent<Genetics>();
            _movement = gameObject.AddComponent<Movement>();
            _genitalia = gameObject.AddComponent<Genitalia>();
            _brain = gameObject.AddComponent<Brain>(); 
            
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _renderer = gameObject.AddComponent<MeshRenderer>();
            _collider = gameObject.AddComponent<MeshCollider>();
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            Debug.Log("Created LifeBaseV3 and components");
        }

        void Start()
        {
            // These don't execute in Awake()
            _meshFilter.mesh = mesh;
            _renderer.material = material;
            
            _collider.sharedMesh = mesh;
            _collider.convex = true;
            
            // We should lock pitch rotation
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            Debug.Log("Started LifeBaseV3");
        }

        void Update()
        {
            Test();
        }

        void Test()
        {
            _movement.MoveTo(new Vector3(Random.Range(5,5),Random.Range(5,5),Random.Range(5,5)));
        }
    }
}
