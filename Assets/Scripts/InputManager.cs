using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class InputManager : MonoBehaviour
    {
        // All input is handled here.
        // This is the only place where input is handled.
        // All other classes should only use this class to get input.

        // We will fire an event when the user presses a key.
        // The event will pass the key that was pressed.
        

        public static InputManager Instance { get; private set; }
        static bool IsInputEnabled { get; set; } = true;

        public Vector2 axes;
        public Vector2 mouseScreenPos;
        public float scrollWheel;
        
        private void Awake()
        {
            if (Instance == null)
            {
                // If there is no instance, set it to this.
                Instance = this;
            }
            else
            {
                // If there is already an instance, destroy this. This should never happen.
                Destroy(gameObject);
            }

            axes = new Vector2();
            mouseScreenPos = new Vector2();
        }

        void Update()
        {
            if (!IsInputEnabled) return;
            GetAxes();
            GetKeyboardInput();
            GetMouseScreenPos();
            GetScrollWheel();
        }

        void GetAxes()
        {
            axes.x = Input.GetAxis("Horizontal");
            axes.y = Input.GetAxis("Vertical");
        }

        void GetMouseScreenPos()
        {
            mouseScreenPos.x = Input.GetAxis("Mouse X");
            mouseScreenPos.y = Input.GetAxis("Mouse Y");
        }

        void GetScrollWheel()
        {
            scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        }
        
        void GetKeyboardInput()
        {
            // Find out if any key is pressed.
            if (!Input.anyKeyDown) return;
            // If any key is pressed, find out which key it is.
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    // If the key is pressed, fire an event.
                    EventManager.current.KeyPressed(key);
                }
            }
        }

        void GetMouseInput()
        {
            
        }
    }
}
