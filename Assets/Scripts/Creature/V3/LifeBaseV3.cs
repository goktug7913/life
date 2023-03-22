using System;
using System.Collections.Generic;
using Creature.V3.Components;
using UnityEngine;

namespace Creature.V3
{
    public class LifeBaseV3 : MonoBehaviour
    {
        /*
         * This is the base class for all life in the game.
         * It is responsible for creating all the components that make up a creature.
         * It also contains the mesh and material for the creature.
         * We don't want to depend on prefabs for this, because we want to be able to
         * create creatures at runtime.
         *
         * We want minimal editing from the Unity Editor, so we will use code to
         * create the components and set their properties.
         *
         * This will result in an easily initialized creature which is created by
         * adding this script to an empty game object.
         */

        // Components will add themselves to this list when they are created
        public List<ComponentBase> components = new List<ComponentBase>();
        
        public Genetics genetics;
        Brain _brain;
        Genitalia _genitalia;
        Movement _movement;
        Sensors _sensors;

        // Unity Components
        MeshFilter _meshFilter;
        MeshRenderer _renderer;
        MeshCollider _collider;
        Rigidbody _rigidbody;
        public Mesh mesh;
        public Material material;

        // TODO: This should be a property of the genetics component probably
        public bool isGenesis = true;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Awake()
        {
            InitializeComponents();
            
            // Set layer to "Life" and tag to "Life"
            gameObject.layer = LayerMask.NameToLayer("Life");
            gameObject.tag = "Life";
            
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
            // TODO: This is the old way of doing things. We should refactor this.
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
    }
}
