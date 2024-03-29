using UnityEngine;

namespace Creature.V3.Components
{
    public class Genetics : ComponentBase
    {
        float[] _dna;
        float _minValue = 0;
        float _maxValue = 1;

        Genus _genus;
        public Sex _sex;
        
        bool _fuse = false;

        void Start()
        {
            base.Start();
            if (_fuse) return;
            if (parentObject.isGenesis)
            {

            }
            _sex = DetermineSex();
        }
        
        public int GetDnaLength()
        {
            return _dna.Length;
        }

        public float[] GetDna()
        {
            return _dna;
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
                _     => Sex.Na,
            };
        }
        
        public Sex GetSex()
        {
            return _sex;
        }
    }

    public class DnaMapper : MonoBehaviour
    {
        // We will use this to provide gene values
        // to other life components, so that they can
        // initialize their attributes.
        
        // This mapping needs to be uniform between creatures
        // Or else, same DNA could generate different attribute values.
        
        
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
