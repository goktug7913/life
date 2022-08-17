using System;
using UnityEngine;

    public class EventManager : MonoBehaviour
    {
        public static EventManager current;

        void Awake()
        {
            current = this;
            Debug.Log("EventManager Awake.");
        }

        public event Action<Vector3, float[]> OnSpawnOffspring;
        public event Action<Vector3> OnSpawnGenesis;
        
        public void RequestLifeSpawn(Vector3 pos, float[] dna)
        {
            //OnSpawnOffspring?.Invoke(pos, dna);
            Debug.Log("OnSpawnOffspring() event");
        }
        
        public void RequestLifeSpawn(Vector3 pos)
        {
            OnSpawnGenesis?.Invoke(pos);
        }
    }