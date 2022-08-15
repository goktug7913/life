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

        public event Action<Vector3, float[]> onSpawnOffspring;
        public event Action<Vector3> onSpawnGenesis;
        
        public void RequestLifeSpawn(Vector3 pos, float[] dna)
        {
            onSpawnOffspring?.Invoke(pos, dna);
        }
        
        public void RequestLifeSpawn(Vector3 pos)
        {
            onSpawnGenesis?.Invoke(pos);
        }
    }