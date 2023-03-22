using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creature.V3.Components
{
    public class Genitalia : ComponentBase
    {
        Genetics _geneticsRef;
        
        bool _hasWomb = false;
        bool _canMate = true;
        
        float[] _partnerDna;
        
        [SerializeField] float mutationAmount = 0.05f;

        float _gestation;
        float _gestationMax;

        float _matingProgress;
        float _matingProgressMax;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Awake()
        {
            _geneticsRef = GetComponent<Genetics>();
            
            // We will initialize based on sex
            switch (_geneticsRef.GetSex())
            {
                case Sex.Male:
                    _hasWomb = false;
                    break;
                case Sex.Female:
                    _hasWomb = true;
                    break;
                case Sex.Na:
                    Debug.Log("Creature has sex: NA.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Start()
        {
            base.Start();
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void SpawnOffspring()
        {
            // We need to call SpawnLife in the Sim Manager..
            EventManager.current.RequestLifeSpawn(transform.root.position, GenerateOffspringDna());
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        float[] GenerateOffspringDna()
        {
            int dnaLength = _geneticsRef.GetDnaLength();
            
            float[] dna1 = _geneticsRef.GetDna();
            float[] dna2 = _partnerDna;

            float[] offspringDna = new float[dnaLength];
            
            for (int i = 0; i < dnaLength; i++)
            {
                // Modulus 2 to alternate the parent genes (mother if even, father if odd) ,
            
                // We are also going to mutate the offspring by a small amount.
                // The mutation is applied by having a coefficient while passing on the float values.

                
                if (i % 2 == 0)
                {
                    offspringDna[i] = dna1[i] * (1 + Random.Range(-mutationAmount, mutationAmount));
                }
                else
                {
                    offspringDna[i] = dna2[i] * (1 + Random.Range(-mutationAmount, mutationAmount));
                }
            }
            return offspringDna;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    }
}
