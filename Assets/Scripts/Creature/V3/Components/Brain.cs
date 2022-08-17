using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creature.V3.Components
{
    public class Brain : MonoBehaviour
    {
        [SerializeField] State _state;

        [SerializeField] Queue<State> _previousStates;
        [SerializeField] public int stateHistoryLength = 30;
        
        Movement _movementRef;
        Sensors _sensorsRef;
        
        void Awake()
        {
            _previousStates = new Queue<State>(stateHistoryLength);
            
        }

        void Update()
        {
            _movementRef.MoveTo(new Vector3(Random.Range(-5,5),Random.Range(-5,5),0.5f));
        }

        void SetReferences()
        {
            _movementRef = GetComponentInParent<Movement>();
            _sensorsRef = GetComponentInParent<Sensors>();
        }
        
        void StateTransition(State oldState, State newState)
        {
            // Every state transition should happen through this function.
            if (oldState == newState) return;
            
            _state = newState;
            AddStateHistory(oldState);
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