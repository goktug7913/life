using UnityEngine;

namespace Creature
{
    public class MovementComponent : MonoBehaviour
    {
        private Attributes _attributes;
        
        public struct Attributes
        {
            private float _speed;
            private float _maxSpeed;
            private float _turnRate;
            private float _acceleration;
            private float _deceleration;
        }
    }
}