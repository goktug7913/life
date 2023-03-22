using UnityEngine;

namespace Creature.V3.Components
{
    public class Movement : ComponentBase
    {

        GameObject _root;
        public Attributes _attributes;
        public Vector3 _target;

        public struct Attributes
        {
            internal float speed;
            internal float maxSpeed;
            internal float turnRate;
            internal float acceleration;
            internal float deceleration;
        }

        void Awake()
        {
            InitDebug(); // We use this for testing movement
            
            // Set the root object
            _root = transform.root.gameObject;
        }

        void Start()
        {
            base.Start();
        }

        void InitDebug()
        {
            _attributes = new Attributes
            {
                speed = 1,
                maxSpeed = 1,
                turnRate = 1,
                acceleration = 1,
                deceleration = 1,
            };
        }
        
        public void MoveTo(Vector3 target)
        {
            Vector3 position = transform.position;
            Vector3 moveDirection = target - position;
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);

            // Do not rotate vertically.
            lookRotation.x = 0;

            // Move the animal in the direction of the wander target.
            position += moveDirection.normalized * (_attributes.speed * Time.deltaTime);
            _root.transform.position = position;
            _root.transform.rotation = Quaternion.Slerp(_root.transform.rotation, lookRotation, Time.deltaTime * (_attributes.turnRate));
        }
        
    }
}