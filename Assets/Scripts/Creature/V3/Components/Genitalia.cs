using System;
using UnityEngine;

namespace Creature.V3.Components
{
    public class Genitalia : MonoBehaviour
    {
        Genetics geneticsRef;
        void Awake()
        {
            geneticsRef = GetComponent<Genetics>();
            
            // We will initialize based on sex
            switch (geneticsRef.GetSex())
            {
                case Sex.Male:
                    break;
                case Sex.Female:
                    break;
                case Sex.Na:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        void SpawnOffspring()
        {
            // We need to call SpawnLife in the Sim Manager..
        }
    }
}
