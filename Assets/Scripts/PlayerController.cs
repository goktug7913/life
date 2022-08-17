using System.Collections.Generic;
using Creature.V3;
using DefaultNamespace;
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

    public bool canMove = true;
    public bool canZoom = true;
    public bool canOrbit = true;

    List<LifeBaseV3> _selectedLife;
    
    // Start is called before the first frame update
    void Start()
    {
        _selectedLife = new List<LifeBaseV3>(); // initialize the list.
        // Funny note: I forgot to initialize this list, and it was causing a null reference exception.
        // It took me a few hours to find out. I was almost deleting the whole selection script.

        // Find the manager in the level
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
        CameraMove();
        CameraOrbit();
        ScrollZoom();
    }

    void SelectLife()
    {
    }

    void CameraMove()
    {
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
    }

    void CameraOrbit()
    {
        if (!canOrbit && !Input.GetMouseButton(2)) return; // If can't orbit, return.

        // get cursor position
        float mouseX = InputManager.Instance.mouseScreenPos.x; // Refactor this TODO
        float mouseY = InputManager.Instance.mouseScreenPos.y; // Refactor this TODO

        // apply inversion if enabled
        if (invertY) mouseY *= -1;

        // mouseX is the horizontal distance, mouseY is the vertical distance.
        Vector3 mouseMovement = new Vector3(mouseY, 0f, mouseX) * (Time.deltaTime * orbitSpeed * _orbitMultiplier);
        cameraRoot.transform.Rotate(mouseY,mouseX,0);

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


