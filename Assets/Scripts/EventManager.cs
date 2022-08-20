using System;
using UnityEngine;

    public class EventManager : MonoBehaviour
    {
        public static EventManager current; // Singleton
        public event Action<Vector3, float[]> OnSpawnOffspring;
        public event Action<Vector3> OnSpawnGenesis;

        public event Action<KeyCode> OnKeyPressed;
        public event Action<KeyCode> OnKeyReleased;
        public event Action<KeyCode> OnKeyHeld;
        public event Action<InputManager.MouseButton> OnMouseButtonPressed;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Awake()
        {
            current = this;
            Debug.Log("EventManager Awake.");
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public void RequestLifeSpawn(Vector3 pos, float[] dna)
        {
            OnSpawnOffspring?.Invoke(pos, dna);
            Debug.Log("OnSpawnOffspring() event");
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public void RequestLifeSpawn(Vector3 pos)
        {
            OnSpawnGenesis?.Invoke(pos);
            Debug.Log("OnSpawnGenesis() event");
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public void KeyPressed(KeyCode key)
        {
            OnKeyPressed?.Invoke(key);
            Debug.Log("OnKeyPressed(): " + key);
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public void KeyReleased(KeyCode key)
        {
            OnKeyReleased?.Invoke(key);
            Debug.Log("OnKeyReleased(): " + key);
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public void KeyHeld(KeyCode key)
        {
            OnKeyHeld?.Invoke(key);
            Debug.Log("OnKeyHeld(): " + key);
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public void MouseButtonPressed(InputManager.MouseButton button)
        {
            OnMouseButtonPressed?.Invoke(button);
            Debug.Log("OnMouseButtonPressed() event");
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    }