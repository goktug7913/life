using System.Collections.Generic;
using UnityEngine;

namespace Creature.V3.Components
{
    public class Brain : ComponentBase
    {
        [SerializeField] State _state;

        [SerializeField] Queue<State> _previousStates;
        [SerializeField] public int stateHistoryLength = 30;
        
        [SerializeField] Movement _movementRef;
        [SerializeField] Sensors _sensorsRef;

        void Awake()
        {
            _previousStates = new Queue<State>(stateHistoryLength);
            SetReferences();
        }

        void Start()
        {
            base.Start();
            StateTransition(State.Wander);
            _movementRef._target = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0.5f);
        }

        void Update()
        {
            if (Mathf.Approximately(_movementRef._target.x, transform.position.x) &&
                Mathf.Approximately(_movementRef._target.y, transform.position.y))
            {
                _movementRef._target = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0.5f);
            }

            _movementRef.MoveTo(_movementRef._target);
        }

        void SetReferences()
        {
            _movementRef = GetComponentInParent<Movement>();
            _sensorsRef = GetComponentInParent<Sensors>();
        }
        
        void StateTransition(State newState)
        {
            // Every state transition should happen through this function.
            State oldState = _state;
            if (oldState == newState) return;
            
            AddStateHistory(oldState);
            _state = newState;
        }

        void AddStateHistory(State state)
        {
            // We handle adding to state history here.
            if (_previousStates.Count < stateHistoryLength)
            {
                _previousStates.Enqueue(state);
            }
            else
            {
                _previousStates.Dequeue();
                _previousStates.Enqueue(state);
            }
        }
        
    }
    
    public enum State {
        Idle,
        Wander,
        
        GoToMate,
        FindMate,
        Mating,
        
        GoToFood,
        FindFood,
        Eating,
        
        Die,
    }
}