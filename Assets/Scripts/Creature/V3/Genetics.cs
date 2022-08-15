using UnityEngine;

namespace Creature.V3
{
    public class Genetics : MonoBehaviour
    {
        float[] _dna;
        float _minValue = 0;
        float _maxValue = 1;

        Genus _genus;
        Sex _sex;
        
        bool _fuse = false;

        public int GetDnaLength()
        {
            return _dna.Length;
        }

        public void SetDna(float[] dna)
        {
            if (_fuse) Debug.LogError(this + " tried to overwrite DNA");
            else
            {
                _dna = new float[dna.Length];
                _dna = dna;

                _sex = DetermineSex();

                _fuse = true; // We blow the fuse, cannot execute this again.
            }
        }

        Sex DetermineSex()
        {
            float val = (_dna[0] + _dna[1])/2;

            return val switch
            {
                < .5f => Sex.Male,
                > .5f => Sex.Female,
                _ => Sex.Na,
            };
        }
        
        public Sex GetSex()
        {
            return _sex;
        }
    }
    
    public enum Genus{
        Animalia,
        Plantae,
        Fungi,
        Na,
        // Note: More in the future?
    }

    // All life will have a sex, even if it doesn't classify as male or female,
    // it will enumerate to NA.
    public enum Sex{
        Male,
        Female,
        Na,
    }
    
}
