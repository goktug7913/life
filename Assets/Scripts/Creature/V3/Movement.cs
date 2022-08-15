using UnityEngine;

namespace Creature.V3
{
    public class Movement : MonoBehaviour
    {
        private Attributes _attributes;

        struct Attributes
        {
            private float _speed;
            private float _maxSpeed;
            private float _turnRate;
            private float _acceleration;
            private float _deceleration;
        }
    }
}