using System.Collections;
using System.Collections.Generic;
using Creature.V3;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerRoot;
    public GameObject cameraRoot;
    public Camera _Camera;

    public float moveSpeed = 3f;
    
    public float zoomSpeed = 10f;
    
    public float orbitSpeed = 1f;
    float _orbitMultiplier = 1000f;
    float _moveMultiplier = 10f;
    
    public bool invertY = true;

    public bool canMove = false;
    public bool canZoom = true;
    public bool canOrbit = true;

    List<LifeBaseV3> _selectedLife;
    
    // References
    InputManager _inputManager;
    SimulationManager _simulationManager;
    EventManager _eventManager;

    // Start is called before the first frame update
    void Start()
    {
        _selectedLife = new List<LifeBaseV3>();
        // Funny note: I forgot to initialize this list, and it was causing a null reference exception.
        // It took me a few hours to find out. I was almost deleting the whole selection script.

        // Find managers in the level
        _inputManager = InputManager.Instance;
        _simulationManager = SimulationManager.Instance;
        _eventManager = EventManager.current;
        
        // Subscribe to events
        _eventManager.OnKeyPressed += CameraMove;
        _eventManager.OnKeyReleased += StopMove;

    }

    // Update is called once per frame
    void Update()
    {
        SelectLife();
        // Spawn a pair of animals on right click.
        if (Input.GetMouseButtonDown(1))
        {
            SpawnLifeV3();
        }
    }

    void FixedUpdate()
    {
        // I moved this to FixedUpdate because it is supposed to be more stable for the camera and movement.
        //CameraMove();
        CameraOrbit();
        ScrollZoom();
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    IEnumerator AsyncMove()
    {
        Debug.Log("AsyncMove");
            while (canMove)
            {
                Vector3 move = new Vector3(_inputManager.axes.x, _inputManager.axes.y, 0);

                // rotate movement with camera.
                move = Quaternion.Euler(0, cameraRoot.transform.rotation.eulerAngles.y, 0) * move;

                // add modifiers and move.
                move *= (moveSpeed * _moveMultiplier * Time.deltaTime);
                playerRoot.transform.Translate(move);
            }
            yield return null;
    }
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // more async here
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    void SelectLife()
    {
    }

    void CameraMove(KeyCode key)
    {
        /*
        if (!canMove){return;} // If can't move, return.
        
        // WASD movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveHorizontal == 0 && moveVertical == 0){return;} // If no input, return.

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // rotate movement with camera.
        movement = Quaternion.Euler(0, cameraRoot.transform.rotation.eulerAngles.y, 0) * movement;

        // add modifiers and move.
        movement = movement * (moveSpeed * _moveMultiplier * Time.deltaTime);
        playerRoot.transform.Translate(movement);
        */

        if (key is not (KeyCode.W or KeyCode.S or KeyCode.A or KeyCode.D)) return;
        
        Debug.Log("CameraMove");
        StartCoroutine(AsyncMove());
    }
    
    void StopMove(KeyCode key)
    {
        if (key is KeyCode.W or KeyCode.S or KeyCode.A or KeyCode.D)
        {
            // This will stop the movement coroutine as the loop will evaluate to false.
            canMove = false;
        }
    }

    void CameraOrbit()
    {
        if (!canOrbit || !Input.GetMouseButton(2)) return; // If can't orbit, return.

        // get cursor position
        Vector2 mouseXY = InputManager.Instance.mouseScreenPos;

        // apply inversion if enabled
        if (invertY) mouseXY.y *= -1;

        // mouseX is the horizontal distance, mouseY is the vertical distance.
        Vector3 mouseMovement = new Vector3(mouseXY.y, 0f, mouseXY.x) * (Time.deltaTime * orbitSpeed * _orbitMultiplier);
        cameraRoot.transform.Rotate(mouseXY.y,mouseXY.x,0);

        // we need to zero out Z axis... (fix later)
        Quaternion rotation = cameraRoot.transform.rotation;
        rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, 0);
        cameraRoot.transform.rotation = rotation;
    }

    void ScrollZoom()
    {
        if (!canZoom){return;} // If can't zoom, return.

        if (InputManager.Instance.scrollWheel == 0){return;} // if no scroll, return.

        // change camera distance based on scroll.
        float zoom = InputManager.Instance.scrollWheel * zoomSpeed;

        // move camera
        _Camera.transform.Translate(0, 0, zoom);
    }
    
    void SpawnLifeV3()
    {
        Vector3 worldPos = _Camera.ScreenToWorldPoint(InputManager.Instance.mouseScreenPos);
        // Move the Y position to the ground.
        worldPos.y = 3;
        
        EventManager.current.RequestLifeSpawn(worldPos);
    }
}


