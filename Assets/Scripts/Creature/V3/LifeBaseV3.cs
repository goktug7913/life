using System;
using Creature.V3.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creature.V3
{
    public class LifeBaseV3 : MonoBehaviour
    {
        public Genetics genetics;
        Brain _brain;
        Genitalia _genitalia;
        Movement _movement;
        Sensors _sensors;
        
        MeshFilter _meshFilter;
        MeshRenderer _renderer;
        MeshCollider _collider;
        Rigidbody _rigidbody;
        
        public Mesh mesh;
        public Material material;

        public bool isGenesis = true;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Awake()
        {
            InitializeComponents();
            Debug.Log("Created LifeBaseV3 and components");
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Start()
        {
            // These don't execute in Awake()
            _meshFilter.mesh = mesh;
            _renderer.material = material;
            
            _collider.sharedMesh = mesh;
            _collider.convex = true;
            
            // Let's color code the sex
            _renderer.material.color = genetics.GetSex() switch
            {
                Sex.Male   => Color.blue,
                Sex.Female => Color.magenta,
                Sex.Na     => Color.gray,
                _          => throw new ArgumentOutOfRangeException(),
            };
            
            // We should lock pitch rotation
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
            Debug.Log("Started LifeBaseV3");
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void InitializeComponents()
        {
            genetics = gameObject.AddComponent<Genetics>();
            _movement = gameObject.AddComponent<Movement>();
            _genitalia = gameObject.AddComponent<Genitalia>();
            _brain = gameObject.AddComponent<Brain>();
            _sensors = gameObject.AddComponent<Sensors>();
            
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _renderer = gameObject.AddComponent<MeshRenderer>();
            _collider = gameObject.AddComponent<MeshCollider>();
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Test()
        {
            _movement.MoveTo(new Vector3(Random.Range(5,5),Random.Range(5,5),Random.Range(5,5)));
        }
    }
}
