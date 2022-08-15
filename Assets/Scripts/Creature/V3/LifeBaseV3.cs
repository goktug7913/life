using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Creature.V3
{
    public class LifeBaseV3 : MonoBehaviour
    {
        public Genetics genetics;
        
        MeshFilter _meshFilter;
        MeshRenderer _renderer;
        MeshCollider _collider;

        public Mesh mesh;
        public Material material;
        
        void Awake()
        {
            genetics = gameObject.AddComponent<Genetics>();
            
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _renderer = gameObject.AddComponent<MeshRenderer>();
            _collider = gameObject.AddComponent<MeshCollider>();

        }

        void Start()
        {
            // These don't execute in Awake()
            _meshFilter.mesh = mesh;
            _renderer.material = material;
            _collider.sharedMesh = mesh;
        }
        
        
    }
}
